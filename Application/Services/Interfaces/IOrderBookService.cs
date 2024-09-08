using InvestimentApi.Application.DTOs;
using InvestimentApi.Domain.Entities;
using InvestimentApi.Domain.Enum;

namespace InvestimentApi.Application.Services.Interfaces
{
    public interface IOrderBookService
    {
        Task AddOrderAsync(OrderDto orderDto);
        Task<OrderCalculationResponseDto> GetBestOrderAsync(string asset, OrderType orderType, decimal quantity);
        Task<List<Order>> GetAsync(string asset, DateTime startTime);
    }
}
