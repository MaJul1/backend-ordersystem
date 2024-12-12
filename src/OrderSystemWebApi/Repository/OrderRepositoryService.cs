using System;
using Microsoft.EntityFrameworkCore;
using OrderSystemWebApi.Context;
using OrderSystemWebApi.DTO.Order;
using OrderSystemWebApi.Interfaces;
using OrderSystemWebApi.Mapper;
using OrderSystemWebApi.Models;

namespace OrderSystemWebApi.Repository;

public class OrderRepositoryService : IOrderRepositoryService
{
    private readonly IProductRepositoryService _productService;
    private readonly OrderSystemContext _context;
    public OrderRepositoryService(IProductRepositoryService productRepositoryService, OrderSystemContext context)
    {
        _productService = productRepositoryService;
        _context = context;
    }

    public async Task<Order> CreateOrder(OrderRequestDTO request, string userId)
    {
        var productIds = request.ProductIds;

        var products = await _productService.GetRangeProductsByIdsAsync(productIds);

        if (products.Count == 0)
            throw new ArgumentNullException(nameof(request), "ProductIds should not be null or empty.");

        var order = OrderMapper.ToOrder(userId, products);

        await _context.Orders.AddAsync(order);

        await _context.SaveChangesAsync();
        
        return order;
    }

    //TODO
    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        var orders = await _context.Orders
            .Include(o => o.Products)
            .ToListAsync();

        return orders;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersByUserIdAsync(string userId)
    {
        var userOrder = await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.Products)
            .ToListAsync();
        
        return userOrder;
    }

    //TODO
    public async Task<Order?> GetOrderById(Guid id)
    {
        var order = _context.Orders
            .Include(o => o.Products)
            .FirstOrDefault(o => o.Id == id);

        return await Task.FromResult(order);
    }
}
