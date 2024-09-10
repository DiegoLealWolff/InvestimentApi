using InvestimentApi.Domain.Entities;
using InvestimentApi.Domain.Enums;
using InvestimentApi.Domain.Interfaces.Strategies;

namespace InvestimentApi.Domain.Strategies
{
    public class SellOrderStrategy : IOrderStrategy
    {
        public  Task<OrderCalculation> GetBestOrdersAsync(List<Order> orders, decimal quantity, string asset)
        {
            List<OrderBookItem> usedOrders = new List<OrderBookItem>();
            decimal totalQuantity = 0;
            decimal totalPrice = 0;

            var bids = orders.Where(item => item.OrderType == OrderType.Sell).OrderByDescending(o => o.Price).ToList();

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

            return Task.FromResult(new OrderCalculation
            {
                OrderCalculationId = Guid.NewGuid(),
                Asset = asset,
                OrderType = OrderType.Sell,
                TotalQuantity = totalQuantity,
                UsedOrders = usedOrders,
                TotalPrice = totalPrice
            });
        }
    }
}
