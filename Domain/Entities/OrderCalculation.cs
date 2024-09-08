using InvestimentApi.Domain.Entities;
using InvestimentApi.Domain.Enum;

public class OrderCalculation
{
    public Guid OrderCalculationId { get; set; }
    public string Asset { get; set; } = string.Empty;
    public OrderType OrderType { get; set; }
    public decimal TotalQuantity { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderBookItem> UsedOrders { get; set; }
}