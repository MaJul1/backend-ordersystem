using System;
using OrderSystemWebApi.DTO.Order;
using OrderSystemWebApi.Models;

namespace OrderSystemWebApi.Interfaces;

public interface IOrderRepositoryService
{
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<IEnumerable<Order>> GetAllUserOrdersAsync(string UserId);
    Task<Order?> GetOrderById(Guid id);
    Task<Order> CreateOrder(OrderRequestDTO request, string userId);
}
