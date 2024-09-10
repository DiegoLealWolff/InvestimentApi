using InvestimentApi.Domain.Enums;

namespace InvestimentApi.Domain.Interfaces.Strategies
{
    public interface IOrderStrategyFactory
    {
        IOrderStrategy GetStrategy(OrderType orderType);
    }
}
