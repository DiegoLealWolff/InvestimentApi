using InvestimentApi.Domain.Entities;
using InvestimentApi.Domain.Enums;

namespace InvestimentApi.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);
        Task<List<Order>> GetOrdersAsync(string asset, OrderType orderType);
        Task<(decimal MinPrice, decimal MaxPrice)> GetMinMaxPricesAsync(string asset);
        Task<List<Order>> GetOrdersAsync(string asset, DateTime startTime, DateTime endTime);
        Task SaveCalculationAsync(OrderCalculation calculation);
    }
}
