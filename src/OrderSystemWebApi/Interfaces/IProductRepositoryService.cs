using System;
using OrderSystemWebApi.DTO;
using OrderSystemWebApi.Models;

namespace OrderSystemWebApi.Interfaces;

public interface IProductRepositoryService
{
    Task<Product> ReadById(Guid id);
    IEnumerable<Product> ReadAll();
    Task<Product> CreateProduct(WriteProductRequestDTO request);
    Task UpdateProduct(Guid productId, WriteProductRequestDTO request);
    Task DeleteProduct(Guid productId);
    Task<bool> IsExistById(Guid id);

}
