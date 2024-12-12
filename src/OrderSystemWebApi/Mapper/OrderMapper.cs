using Microsoft.Extensions.Diagnostics.HealthChecks;
using OrderSystemWebApi.DTO.Order;
using OrderSystemWebApi.DTO.Product;
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

    public static ReadOrderRequestDTO ToReadOrderDTO(this Order order)
    {
        ReadOrderRequestDTO ReadOrder = new()
        {
            Id = order.Id,
            Buyer = Guid.Parse(order.UserId)
        };

        List<ReadProductRequestDTO> products = [];
        
        foreach(Product product in order.Products)
        {
            products.Add(product.ToReadProductDTO());
        }

        ReadOrder.ProductsOrdered = products;

        return ReadOrder;
    }
}
