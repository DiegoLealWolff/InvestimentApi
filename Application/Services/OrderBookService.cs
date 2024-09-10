using InvestimentApi.Application.DTOs;
using InvestimentApi.Application.Services.Interfaces;
using InvestimentApi.Domain.Entities;
using InvestimentApi.Domain.Enums;
using InvestimentApi.Domain.Factories;
using InvestimentApi.Domain.Interfaces;
using InvestimentApi.Domain.Interfaces.Strategies;

namespace InvestimentApi.Application.Services
{
    public class OrderBookService : IOrderBookService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderStrategyFactory _strategyFactory;
        private readonly OrderFactory _orderFactory;

        public OrderBookService(IOrderRepository orderRepository, IOrderStrategyFactory strategyFactory, OrderFactory orderFactory)
        {
            _orderRepository = orderRepository;
            _strategyFactory = strategyFactory;
            _orderFactory = orderFactory;
        }      
        public async Task<OrderCalculationDto> GetBestOrderAsync(string asset, OrderType orderType, decimal quantity)
        {
            var orderBook = await _orderRepository.GetOrdersAsync(asset, orderType);
            if (orderBook == null)
            {
                throw new Exception($"OrderBook for {asset} not found.");
            }

            var strategy = _strategyFactory.GetStrategy(orderType);
            var bestOrders = await strategy.GetBestOrdersAsync(orderBook, quantity, asset);
          
            if (bestOrders.TotalQuantity < quantity)
            {
                throw new Exception($"Insufficient liquidity to fulfill {quantity} {asset}.");
            }        

            SaveCalculationAsync(bestOrders);

            return new OrderCalculationDto
            {
                OrderCalculationId = bestOrders.OrderCalculationId,
                Asset = bestOrders.Asset,
                OrderType = bestOrders.OrderType.ToString(),
                Quantity = bestOrders.TotalQuantity,
                UsedOrders = bestOrders.UsedOrders,
                TotalPrice = bestOrders.TotalPrice
            };
        }

        public async Task AddOrderAsync(OrderDto orderDto)
        {        
            var order = _orderFactory.CreateOrder(orderDto);
            await _orderRepository.AddOrderAsync(order);
        }

        public async Task<List<Order>> GetOrdersAsync(string asset, DateTime startTime)
        {
            return await _orderRepository.GetOrdersAsync(asset, startTime, DateTime.Now);
        }

        public async void SaveCalculationAsync(OrderCalculation orderCalculation)
        {
            await _orderRepository.SaveCalculationAsync(orderCalculation);
        }
    }
}
