using OrderSystemWebApi.DTO.Product;
using OrderSystemWebApi.Models;

using OrderSystemWebApi.Query.ProductQuery;

namespace OrderSystemWebApi.Interfaces;

public interface IProductRepositoryService
{
    Task<Product?> GetByIdAsync(Guid id);
    IEnumerable<Product> GetAll();
    IEnumerable<Product> GetAll(ProductQueryOptions options);
    Task<Product> CreateProduct(WriteProductRequestDTO request);
    Task UpdateProductAsync(Guid productId, WriteProductRequestDTO request);
    Task DeleteProductAsync(Guid productId);
    Task<List<Product>> GetRangeProductsByIdAsync(Guid[] ids);
}
