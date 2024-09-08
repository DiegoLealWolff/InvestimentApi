using InvestimentApi.Domain.Enum;

namespace InvestimentApi.Application.DTOs
{
    public class OrderDto
    {
        public string Asset { get; set; } = string.Empty;
        public OrderType OrderType { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
    }
}
