﻿using InvestimentApi.Data;
using InvestimentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InvestimentApi.Repositories
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

        public async Task<List<Order>> GetOrdersAsync(string assetPair, OrderType orderType)
        {
            return await _context.Orders
                .Where(o => o.Asset == assetPair && o.OrderType == orderType)
                .OrderBy(o => o.Price)
                .ToListAsync();
        }

        public async Task<(decimal MinPrice, decimal MaxPrice)> GetMinMaxPricesAsync(string assetPair)
        {
            var minPrice = await _context.Orders
                .Where(o => o.Asset == assetPair)
                .MinAsync(o => o.Price);
            var maxPrice = await _context.Orders
                .Where(o => o.Asset == assetPair)
                .MaxAsync(o => o.Price);

            return (minPrice, maxPrice);
        }

        public async Task<List<Order>> GetAsync(string assetPair, DateTime startTime, DateTime endTime)
        {
            return await _context.Orders
            .Where(o => o.Asset == assetPair && o.Date >= startTime && o.Date <= endTime)
            .ToListAsync();
        }

        public async Task SaveCalculationAsync(OrderCalculation calculation)
        {
            
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {                    
                    _context.OrderCalculations.Add(calculation);
                    await _context.SaveChangesAsync();                

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
