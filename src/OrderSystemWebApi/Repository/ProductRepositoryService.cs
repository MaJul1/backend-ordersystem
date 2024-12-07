using System;
using Microsoft.EntityFrameworkCore;
using OrderSystemWebApi.Context;
using OrderSystemWebApi.DTO;
using OrderSystemWebApi.Interfaces;
using OrderSystemWebApi.Mapper;
using OrderSystemWebApi.Models;

namespace OrderSystemWebApi.Repository;

public class ProductRepositoryService : IProductRepositoryService
{
    private OrderSystemContext _context;

    public ProductRepositoryService(OrderSystemContext context)
    {
        _context = context;
    }
    
    public async Task<Product> CreateProduct(WriteProductRequestDTO request)
    {
        var product = request.ToProduct();
        await _context.Products.AddAsync(product);
        return product;
    }

    public async Task DeleteProduct(Guid productId)
    {
        var product = await ReadById(productId);
        _context.Remove(product);
    }

    public async Task<bool> IsExistById(Guid id)
    {
        return await _context.Products.AnyAsync(p => p.Id == id);
    }

    public IEnumerable<Product> ReadAll()
    {
        return _context.Products;
    }

    public async Task<Product> ReadById(Guid id)
    {
        var product = await _context.Products.FindAsync(id) ?? throw Null(nameof(id));
        return product;
    }

    public async Task UpdateProduct(Guid productId, WriteProductRequestDTO request)
    {
        var product = await ReadById(productId);

        _context.Products.Entry(product).CurrentValues.SetValues(request);
    }

    private ArgumentNullException Null(string type)
    {
        return new ArgumentNullException($"'{type}' not found.");

    }
}
