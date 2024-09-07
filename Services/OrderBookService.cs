using InvestimentApi.Models;
using InvestimentApi.Repositories;

namespace InvestimentApi.Services
{
    public class OrderBookService : IOrderBookService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderBookService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task AddOrderAsync(OrderDto orderDto)
        {
            var order = new Order
            {
                Asset = orderDto.Asset,
                OrderType = orderDto.OrderType,
                Price = orderDto.Price,
                Quantity = orderDto.Quantity,
                Date = DateTime.Now
            };

            await _orderRepository.AddOrderAsync(order);
        }

        public async Task<List<Order>> GetAsync(string asset, DateTime startTime)
        {           
            return await _orderRepository.GetAsync(asset, startTime, DateTime.Now);
        }

        public async Task<OrderCalculationResponseDto> GetBestOrderAsync(string asset, OrderType orderType, decimal quantity)
        {
            var orderBook = await _orderRepository.GetOrdersAsync(asset, orderType);
            if (orderBook == null)
            {
                throw new Exception($"OrderBook for {asset} not found.");
            }

            List<OrderBookItem> usedOrders = new List<OrderBookItem>();
            decimal totalQuantity = 0;
            decimal totalPrice = 0;

            if (orderType == OrderType.Buy)
            {                
                var asks = orderBook.Where(item => item.OrderType == OrderType.Buy).OrderBy(o => o.Price).ToList();

                foreach (var ask in asks)
                {
                    if (totalQuantity + ask.Quantity >= quantity)
                    {
                        var remainingQuantity = quantity - totalQuantity;
                        totalPrice += remainingQuantity * ask.Price;
                        totalQuantity += remainingQuantity;

                        usedOrders.Add(new OrderBookItem { Price = ask.Price, Quantity = remainingQuantity });
                        break;
                    }
                    else
                    {
                        totalPrice += ask.Quantity * ask.Price;
                        totalQuantity += ask.Quantity;

                        usedOrders.Add(new OrderBookItem { Price = ask.Price, Quantity = ask.Quantity });
                    }
                }
            }
            else if (orderType == OrderType.Sell)
            {                
                var bids = orderBook.Where(item => item.OrderType == OrderType.Sell).OrderByDescending(o => o.Price).ToList();

                foreach (var bid in bids)
                {
                    if (totalQuantity + bid.Quantity >= quantity)
                    {
                        var remainingQuantity = quantity - totalQuantity;
                        totalPrice += remainingQuantity * bid.Price;
                        totalQuantity += remainingQuantity;

                        usedOrders.Add(new OrderBookItem { Price = bid.Price, Quantity = remainingQuantity });
                        break;
                    }
                    else
                    {
                        totalPrice += bid.Quantity * bid.Price;
                        totalQuantity += bid.Quantity;

                        usedOrders.Add(new OrderBookItem { Price = bid.Price, Quantity = bid.Quantity });
                    }
                }
            }
            else
            {
                throw new Exception("Invalid operation type. Use 'buy' or 'sell'.");
            }

            if (totalQuantity < quantity)
            {
                throw new Exception($"Insufficient liquidity to fulfill {quantity} {asset}.");
            }
           
            var orderCalculationId = Guid.NewGuid();
           
            await _orderRepository.SaveCalculationAsync(new OrderCalculation
            {
                OrderCalculationId = orderCalculationId,
                Asset = asset,
                OrderType = orderType,
                TotalQuantity = quantity,
                TotalPrice = totalPrice,
                UsedOrders = usedOrders
            });
            
            return new OrderCalculationResponseDto
            {
                OrderCalculationId = orderCalculationId,
                Asset = asset,
                OrderType = orderType.ToString(),
                Quantity = quantity,
                UsedOrders = usedOrders,
                TotalPrice = totalPrice
            };
        }
    }
}
