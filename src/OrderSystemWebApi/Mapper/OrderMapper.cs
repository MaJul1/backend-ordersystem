using OrderSystemWebApi.DTO.Order;
using OrderSystemWebApi.Interfaces;
using OrderSystemWebApi.Models;

namespace OrderSystemWebApi.Mapper;

public static class OrderMapper
{
    public static Order ToOrder(string userId, ICollection<Product> products)
    {
        Order order = new()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Products = products
        };

        return order;
    }
}
