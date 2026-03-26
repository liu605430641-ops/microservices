namespace Zhaoxi.MSACommerce.PaymentService.UseCases;

public record OrderDto
{
    public long Id { get; set; }
    // 实付金额
    public long ActualPay { get; set; }
}