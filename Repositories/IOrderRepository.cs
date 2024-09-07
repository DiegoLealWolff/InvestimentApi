using InvestimentApi.Models;

namespace InvestimentApi.Repositories
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);
        Task<List<Order>> GetOrdersAsync(string asset, OrderType orderType);
        Task<(decimal MinPrice, decimal MaxPrice)> GetMinMaxPricesAsync(string asset);
        Task<List<Order>> GetAsync(string asset, DateTime startTime, DateTime endTime);
        Task SaveCalculationAsync(OrderCalculation calculation);
    }
}
