namespace InvestimentApi.Models
{
    public class OrderCalculationRequestDto
    {
        public string Asset { get; set; }  // Ex: "BTC/USD" ou "ETH/USD"
        public string Operation { get; set; }  // "buy" ou "sell"
        public decimal Quantity { get; set; }  // Quantidade de BTC ou ETH
    }
}
