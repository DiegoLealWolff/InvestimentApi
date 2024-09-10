using InvestimentApi.Domain.Entities;

namespace InvestimentApi.Domain.Interfaces.Strategies
{
    public interface IOrderStrategy
    {
        Task<OrderCalculation> GetBestOrdersAsync(List<Order> orders, decimal quantity, string asset);
    }
}
