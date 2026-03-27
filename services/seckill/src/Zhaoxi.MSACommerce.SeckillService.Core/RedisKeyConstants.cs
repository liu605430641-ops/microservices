namespace Zhaoxi.MSACommerce.SeckillService.Core;

public static class RedisKeyConstants
{
    public const string SeckillDatePrefix = "SecKill:Product:";
    public const string SecKillStockPrefix = "SecKill:ProductStock:";
    public const string SecKillStockListPrefix = "SecKill:ProductStockList:";
    public const string SecKillVerifyCodePrefix = "SecKill:VerifyCode:";
    public const string SecKillLinkPrefix = "SecKill:Link:";
    
    public const string SecKillUserQueueCountPrefix = "SecKill:UserQueueCount:";
    public const string SecKillQueueList = "SecKill:Queue:List";
    public const string SecKillQueueStatus = "SecKill:Queue:Status";
    public const string SecKillOrder = "SecKill:Order";
}
