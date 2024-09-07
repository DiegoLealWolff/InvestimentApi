namespace InvestimentApi.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string Asset { get; set; } = string.Empty;
        public OrderType OrderType { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public DateTime Date { get; set; }
    }
}
