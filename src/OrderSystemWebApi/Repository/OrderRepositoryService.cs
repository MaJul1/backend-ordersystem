using System;
using OrderSystemWebApi.Context;
using OrderSystemWebApi.DTO.Order;
using OrderSystemWebApi.Interfaces;
using OrderSystemWebApi.Mapper;
using OrderSystemWebApi.Models;

namespace OrderSystemWebApi.Repository;

public class OrderRepositoryService : IOrderRepositoryService
{
    private readonly IProductRepositoryService _productService;
    public OrderRepositoryService(IProductRepositoryService productRepositoryService)
    {
        _productService = productRepositoryService;
    }

    public async Task<Order> CreateOrder(OrderRequestDTO request, string userId)
    {
        var productIds = request.ProductIds;

        var products = await _productService.GetRangeProductsByIdsAsync(productIds);

        if (products.Count == 0)
            throw new ArgumentNullException(nameof(request), "ProductIds should not be null or empty.");

        var order = OrderMapper.ToOrder(userId, products);
        
        return order;
    }

    //TODO
    public Task<IEnumerable<Order?>> GetAllOrders()
    {
        throw new NotImplementedException();
    }

    //TODO
    public Task<Order> GetOrderById(Guid id)
    {
        throw new NotImplementedException();
    }
}
