using InvestimentApi.Domain.Entities;
using InvestimentApi.Domain.Enums;
using InvestimentApi.Domain.Interfaces.Strategies;

namespace InvestimentApi.Domain.Strategies
{
    public class BuyOrderStrategy : IOrderStrategy
    {
        public Task<OrderCalculation> GetBestOrdersAsync(List<Order> orders, decimal quantity, string asset)
        {            
            List<OrderBookItem> usedOrders = new List<OrderBookItem>();
            decimal totalQuantity = 0;
            decimal totalPrice = 0;

            var asks = orders.Where(item => item.OrderType == OrderType.Buy).OrderBy(o => o.Price).ToList();

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

            return Task.FromResult(new OrderCalculation
            {
                OrderCalculationId = Guid.NewGuid(),
                Asset = asset,
                OrderType = OrderType.Buy,
                TotalQuantity = totalQuantity,
                UsedOrders = usedOrders,
                TotalPrice = totalPrice
            }); 
        }
    }
}
