using OrderSystemWebApi.DTO.Product;
using OrderSystemWebApi.Models;

namespace OrderSystemWebApi.Interfaces;

public interface IProductRepositoryService
{
    Task<Product?> GetByIdAsync(Guid id);
    IEnumerable<Product> GetAll();
    Task<Product> CreateProduct(WriteProductRequestDTO request);
    Task UpdateProductAsync(Guid productId, WriteProductRequestDTO request);
    Task DeleteProductAsync(Guid productId);
    Task<List<Product>> GetRangeProductsByIdsAsync(Guid[] ids);
}
