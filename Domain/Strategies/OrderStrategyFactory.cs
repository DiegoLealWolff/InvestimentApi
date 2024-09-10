using InvestimentApi.Domain.Enums;
using InvestimentApi.Domain.Interfaces.Strategies;

namespace InvestimentApi.Domain.Strategies
{
    public class OrderStrategyFactory : IOrderStrategyFactory
    {
        public IOrderStrategy GetStrategy(OrderType orderType)
        {
            return orderType switch
            {
                OrderType.Buy => new BuyOrderStrategy(),
                OrderType.Sell => new SellOrderStrategy(),
                _ => throw new ArgumentException("Invalid order type")
            };
        }
    }
}
