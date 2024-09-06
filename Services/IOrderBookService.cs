using InvestimentApi.Data;
using InvestimentApi.Models;

namespace InvestimentApi.Services
{
    public interface IOrderBookService
    {
        Task AddOrderAsync(OrderDto orderDto);
        Task<OrderCalculationResponseDto> GetBestOrderAsync(string assetPair, OrderType orderType, decimal quantity);
        Task<List<Order>> GetAsync(string assetPair, DateTime startTime);
    }
}
