using InvestimentApi.Models;

namespace InvestimentApi.Services
{
    public interface IOrderBookService
    {
        Task AddOrderAsync(OrderDto orderDto);
        Task<OrderCalculationResponseDto> GetBestOrderAsync(string asset, OrderType orderType, decimal quantity);
        Task<List<Order>> GetAsync(string asset, DateTime startTime);
    }
}
