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

        await _context.SaveChangesAsync();
        
        return product;
    }

    public async Task DeleteProductAsync(Guid productId)
    {
        var product = await GetByIdAsync(productId) ?? throw Null("Id");

        _context.Remove(product);

        await _context.SaveChangesAsync();
    }

    public IEnumerable<Product> GetAll()
    {
        return _context.Products;
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);

        return product;
    }

    public async Task UpdateProductAsync(Guid productId, WriteProductRequestDTO request)
    {
        var product = await GetByIdAsync(productId) ?? throw Null("Id");

        _context.Products.Entry(product).CurrentValues.SetValues(request);

        await _context.SaveChangesAsync();
    }

    private ArgumentNullException Null(string type)
    {
        return new ArgumentNullException($"'{type}' not found.");
    }
}
