using InvestimentApi.Application.DTOs;
using InvestimentApi.Domain.Entities;
using InvestimentApi.Domain.Enums;

namespace InvestimentApi.Application.Services.Interfaces
{
    public interface IOrderBookService
    {
        Task AddOrderAsync(OrderDto orderDto);
        Task<OrderCalculationDto> GetBestOrderAsync(string asset, OrderType orderType, decimal quantity);
        Task<List<Order>> GetOrdersAsync(string asset, DateTime startTime);
    }
}
