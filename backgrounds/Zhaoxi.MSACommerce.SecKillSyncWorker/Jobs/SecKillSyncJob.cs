using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Quartz;
using StackExchange.Redis;
using Zhaoxi.MSACommerce.SeckillService.Core;
using Zhaoxi.MSACommerce.SeckillService.Core.Entities;
using Zhaoxi.MSACommerce.SeckillService.Infrastructure;
using Zhaoxi.MSACommerce.SeckillService.Infrastructure.Data;
using JsonSerializerOptions = System.Text.Json.JsonSerializerOptions;

namespace Zhaoxi.MSACommerce.SecKillSyncWorker.Jobs;

[DisallowConcurrentExecution]
public class SecKillSyncJob(SecKillDbContext dbContext, IConnectionMultiplexer redis) : IJob
{
    public static readonly JobKey Key = new(nameof(SecKillSyncJob), nameof(SecKillSyncWorker));

    private readonly IDatabase _redisDb = redis.GetDatabase();
    
    public Task Execute(IJobExecutionContext context)
    {
        var beginTimes = SecKillDate.GetBeginTimes();
        
        foreach (var startTime in beginTimes)
        {
            // 时间格式为[SecKill:2024120420]
            var seckillDate = $"{RedisKeyConstants.SeckillDatePrefix}{startTime.ToSecKillTime()}";
            
            // 获取符合条件的商品
            // 1.根据时间段数据查询对应的秒杀商品数据
            // 2.库存>0
            // 3.开始时间>=活动开始时间
            // 4.活动结束时间<开始时间+2小时
            // 5.排除之前已经加载到Redis缓存中的商品数据

            var query = dbContext.SecKillProducts.Where(x => 
                x.Num > 0 &&
                x.StartTime >= startTime && x.EndTime < startTime.AddHours(2)
            );
            //把query的sql语句打印出来
            var sql = query.ToQueryString();
            Console.WriteLine("SecKillSyncJob SQL: " + sql);
            
            
            // 获取redis中指定HashId的keys
            var keys = _redisDb.HashKeys(seckillDate).Select(x=>Convert.ToInt64(x)).ToArray();
            if (keys.Length > 0)
            {
                query = query.Where(x => !keys.Contains(x.Id));
            }
            
            // 查询数据
            var secKillList = query.ToList();

            //将秒杀商品数据存入到Redis缓存
            foreach (var secKill in secKillList)
            {
                // 1.将秒杀商品数据存入到Redis缓存
                // 2.将秒杀商品数据压入到队列中
                // 3.将秒杀商品数据压入到计数器中
           
                var json = JsonSerializer.Serialize(secKill, new JsonSerializerOptions
                {
                   Converters = { new JsonStringEnumConverter() },
                   NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
                });
                _redisDb.HashSetAsync(seckillDate, secKill.Id.ToString(), json);
                _redisDb.KeyExpire(seckillDate, startTime.ToUniversalTime().AddHours(2));

                ///////////////////////// 解决超卖问题 /////////////////////////////
                //商品数据压入队列中
                CreateSekKillNumList(secKill);
                //添加一个计数器 (key:商品的ID  value : 库存数)
                _redisDb.StringIncrement($"{RedisKeyConstants.SecKillStockPrefix}{secKill.Id}", secKill.StockCount);
            }
        }
        
        return Task.CompletedTask;
    }

    private void CreateSekKillNumList(SecKillProduct secKill)
    {
        //创建redis的队列(每一种商品就是一个队列,队列的元素的个数和商品的库存一致) 压入队列
        for (var i = 0; i < secKill.StockCount; i++)
        {   // 5
            // List SeckillGoodsCountList__1001 {1001,1001,1001,1001,1001} // 内存队列
            _redisDb.ListLeftPush($"{RedisKeyConstants.SecKillStockListPrefix}{secKill.Id}", secKill.Id);
        }
    }
}