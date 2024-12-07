using System;
using OrderSystemWebApi.DTO;
using OrderSystemWebApi.Models;

namespace OrderSystemWebApi.Interfaces;

public interface IProductRepositoryService
{
    Task<Product?> GetByIdAsync(Guid id);
    IEnumerable<Product> GetAll();
    Task<Product> CreateProduct(WriteProductRequestDTO request);
    Task UpdateProductAsync(Guid productId, WriteProductRequestDTO request);
    Task DeleteProductAsync(Guid productId);
}
