using InvestimentApi.Domain.Entities;

namespace InvestimentApi.Application.DTOs
{
    public class OrderCalculationResponseDto
    {
        public Guid OrderCalculationId { get; set; }
        public string? Asset { get; set; }
        public string? OrderType { get; set; }
        public decimal Quantity { get; set; }
        public List<OrderBookItem>? UsedOrders { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
