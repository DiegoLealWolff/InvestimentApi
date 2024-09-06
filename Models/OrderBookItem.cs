using System.Text.Json.Serialization;

namespace InvestimentApi.Models
{
    public class OrderBookItem
    {
        [JsonIgnore]
        public int OrderBookItemId { get; set; } 
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        [JsonIgnore]
        public Guid OrderCalculationId { get; set; }      
    }
}
