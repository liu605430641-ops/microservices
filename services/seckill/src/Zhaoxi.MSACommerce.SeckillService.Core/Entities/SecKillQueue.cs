using System.Text.Json.Serialization;
using Zhaoxi.MSACommerce.SeckillService.Core.Enums;

namespace Zhaoxi.MSACommerce.SeckillService.Core.Entities;

public record SecKillQueue
{
    public long UserId { set; get; }

    //秒杀用户名
    public string? Username { set; get; }

    //创建时间
    public DateTime CreateTime { set; get; } = DateTime.Now;

    //秒杀状态  1:排队中，2:秒杀等待支付,3:支付超时，4:秒杀失败,5:支付完成
    public SecKillStatus Status { set; get; } = SecKillStatus.Queuing;

    //秒杀商品ID
    public long SecKillId { set; get; }

    //应付金额
    public long Price { set; get; }

    //订单编号
    public long OrderId { set; get; }

    //秒杀场次
    public required string Time { set; get; }
};