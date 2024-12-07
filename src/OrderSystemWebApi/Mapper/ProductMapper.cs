using System;
using OrderSystemWebApi.DTO;
using OrderSystemWebApi.Models;

namespace OrderSystemWebApi.Mapper;

public static class ProductMapper
{
    public static Product ToProduct(this WriteProductRequestDTO request)
        => ToProduct(request, null);

    public static Product ToProduct(this WriteProductRequestDTO request, Guid? id)
    {
        Product product = new()
        {
            Id = id ?? Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price
        };

        return product;
    }

    public static ReadProductRequestDTO ToReadProductDTO(this Product product)
    {
        ReadProductRequestDTO readProduct = new()
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };

        return readProduct;
    }
}
