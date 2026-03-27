using System.Text.Json;
using System.Text.Json.Serialization;
using DotNetCore.CAP;
using IdGen;
using MediatR;
using StackExchange.Redis;
using Zhaoxi.MSACommerce.SeckillService.Core;
using Zhaoxi.MSACommerce.SeckillService.Core.Entities;
using Zhaoxi.MSACommerce.SeckillService.Core.Enums;
using Zhaoxi.MSACommerce.SeckillService.Infrastructure.Data;
using Zhaoxi.MSACommerce.SharedEvent.SecKills;

namespace Zhaoxi.MSACommerce.SeckillService.UseCases;

public class MultiThreadingCreateOrder(
    IConnectionMultiplexer redis,
    SecKillDbContext dbContext,
    IIdGenerator<long> idGen,
    ICapPublisher capPublisher
    )
{
    private readonly IDatabase _redisDb = redis.GetDatabase();

    /// <summary>
    /// 多线程下单操作[真正抢单过程处理]
    /// </summary>
    public void CreateOrder()
    {
        var jsonOptions = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
        };
        //开启线程 内部已经配置了线程池
        Task.Run(() =>
        {
            // 从排队List中获取排队的信息（左边存储，右边取）
            var secKillQueueValue = _redisDb.ListRightPop(RedisKeyConstants.SecKillQueueList);
            if (secKillQueueValue.IsNullOrEmpty) return;

            try
            {
                // 从redis中取出obj并反序列化为SeckillStatus
                var seckillStatus = JsonSerializer.Deserialize<SecKillQueue>(secKillQueueValue, jsonOptions);
                if (seckillStatus == null) return;

                //获取排队信息
                var time = seckillStatus.Time;
                var id = seckillStatus.SecKillId; //秒杀商品的ID
                var username = seckillStatus.Username;
                var userId = seckillStatus.UserId;

                //判断 先从队列中获取商品 ,如果能获取到,说明 有库存,如果获取不到,说明 没库存 卖完了 return.
                // 多线程竞争库存（单线程队列）
                var ele = _redisDb.ListRightPop($"{RedisKeyConstants.SecKillStockListPrefix}{id}");
                if (ele.IsNull)
                {
                    //卖完了
                    //清除排队状态标识  防止重复排队的key
                    _redisDb.HashDelete(RedisKeyConstants.SecKillQueueStatus, userId);
                    throw new Exception("秒杀活动结束!");
                }

                // 获取商品详情数据
                var productValue = _redisDb.HashGet($"{RedisKeyConstants.SeckillDatePrefix}{time}", id);

                if (productValue.IsNullOrEmpty) throw new Exception("秒杀商品已售罄!");

                var product = JsonSerializer.Deserialize<SecKillProduct>(productValue!, jsonOptions);

                var seckillOrder = new SeckillOrder()
                {
                    Id = idGen.CreateId(), // 通过雪花算法
                    SeckillId = id,
                    ActualPay = product.Price,
                    UserId = userId,
                    CreateTime = DateTime.Now,
                    Status = OrderStatus.UnPay
                };
                //将秒杀订单存入到Redis中
                _redisDb.HashSet(RedisKeyConstants.SecKillOrder, userId,
                    JsonSerializer.Serialize(seckillOrder, jsonOptions));

                //5.减库存
                var stock = _redisDb.StringIncrement($"{RedisKeyConstants.SecKillStockPrefix}{id}", -1);
                product.StockCount = stock;
                product.Num = product.Num++;

                //判断当前商品是否还有库存
                if (product.StockCount <= 0)
                {
                    //将商品数据同步到MySQL中
                    dbContext.SecKillProducts.Update(product);
                }
                else
                {
                    //如果有库存，则将数据重置到Redis
                    _redisDb.HashSet($"{RedisKeyConstants.SeckillDatePrefix}{time}", id,
                        JsonSerializer.Serialize(product, jsonOptions));
                }

                // 修改抢购状态
                // 抢单成功，更新抢单状态,排队->等待支付
                seckillStatus.Status = SecKillStatus.UnPayment;
                seckillStatus.OrderId = seckillOrder.Id;
                seckillStatus.Price = seckillOrder.ActualPay;
                _redisDb.HashSet(RedisKeyConstants.SecKillQueueStatus, userId, JsonSerializer.Serialize(seckillStatus,
                    jsonOptions
                ));

                // 发布超时订单处理的消息。 【超时支付订单逻辑完全一样】
                capPublisher.PublishDelay(TimeSpan.FromSeconds(15), nameof(SecKillTimeoutEvent),
                    new SecKillTimeoutEvent(userId, seckillOrder.Id));

                Console.WriteLine($"秒杀库存剩余量为：{product.StockCount}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("子线程已经执行完成");
        });
    }
}