using System;
using OrderSystemWebApi.DTO.Order;
using OrderSystemWebApi.Models;

namespace OrderSystemWebApi.Interfaces;

public interface IOrderRepositoryService
{
    Task<IEnumerable<Order?>> GetAllOrders();
    Task<Order> GetOrderById(Guid id);
    Task<Order> CreateOrder(OrderRequestDTO request, string userId);
}
