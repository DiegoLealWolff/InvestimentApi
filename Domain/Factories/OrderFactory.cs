using InvestimentApi.Application.DTOs;
using InvestimentApi.Domain.Entities;

namespace InvestimentApi.Domain.Factories
{
    public class OrderFactory
    {
        public Order CreateOrder(OrderDto orderDto)
        {
            return new Order
            {
                Asset = orderDto.Asset,
                OrderType = orderDto.OrderType,
                Price = orderDto.Price,
                Quantity = orderDto.Quantity,
                Date = DateTime.Now
            };
        }
    }
}
