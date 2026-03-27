namespace Zhaoxi.MSACommerce.SeckillService.Core.Enums;

/// <summary>
/// //秒杀状态  1:排队中，2:等待支付,3:支付超时，4:秒杀失败,5:支付完成
/// </summary>
public enum SecKillStatus
{
    Queuing = 1,
    UnPayment = 2,
    Timeout = 3,
    Failed = 4,
    Completed = 5
}