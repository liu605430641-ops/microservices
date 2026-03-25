namespace Zhaoxi.MSACommerce.SharedEvent.Orders;

public record OrderPayedEvent
{
    public long OrderId { get; set; }
};