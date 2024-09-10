using InvestimentApi.Domain.Entities;
using InvestimentApi.Domain.Enums;
using InvestimentApi.Domain.Interfaces;
using InvestimentApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvestimentApi.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetOrdersAsync(string asset, OrderType orderType)
        {
            return await _context.Orders
                .Where(o => o.Asset == asset && o.OrderType == orderType)
                .OrderBy(o => o.Price)
                .ToListAsync();
        }

        public async Task<(decimal MinPrice, decimal MaxPrice)> GetMinMaxPricesAsync(string asset)
        {
            var minPrice = await _context.Orders
                .Where(o => o.Asset == asset)
                .MinAsync(o => o.Price);
            var maxPrice = await _context.Orders
                .Where(o => o.Asset == asset)
                .MaxAsync(o => o.Price);

            return (minPrice, maxPrice);
        }

        public async Task<List<Order>> GetOrdersAsync(string asset, DateTime startTime, DateTime endTime)
        {
            return await _context.Orders
            .Where(o => o.Asset == asset && o.Date >= startTime && o.Date <= endTime)
            .ToListAsync();
        }

        public async Task SaveCalculationAsync(OrderCalculation calculation)
        {
            try
            {
                _context.OrderCalculations.Add(calculation);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
