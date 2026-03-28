namespace Zhaoxi.MSACommerce.PaymentService.UseCases;

public record SecKillOrderDto
{
    public required string Id { get; set; }
    // 实付金额
    public required string ActualPay { get; set; }
}