using InvestimentApi.Data;
using InvestimentApi.Models;

namespace InvestimentApi.Repositories
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);
        Task<List<Order>> GetOrdersAsync(string assetPair, OrderType orderType);
        Task<(decimal MinPrice, decimal MaxPrice)> GetMinMaxPricesAsync(string assetPair);
        Task<List<Order>> GetAsync(string assetPair, DateTime startTime, DateTime endTime);
        Task SaveCalculationAsync(OrderCalculation calculation);
    }
}
