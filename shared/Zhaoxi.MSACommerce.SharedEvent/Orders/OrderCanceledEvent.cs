namespace Zhaoxi.MSACommerce.SharedEvent.Orders;

public record OrderCanceledEvent
{
    public long OrderId { get; set; }
};