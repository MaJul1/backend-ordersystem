using OrderSystemWebApi.Models;
using OrderSystemWebApi.Query.Service;

namespace OrderSystemWebApi.Query;

public static class ProductQueryService
{
    public static IEnumerable<Product> ApplyQueryService(this IEnumerable<Product> products, ProductQueryOptions options)
    {
        return products.ApplyFilter(options).ApplySort(options).ApplyPagination(options);
    }
    
    private static IEnumerable<Product> ApplyFilter(this IEnumerable<Product> products, ProductQueryOptions options)
    {
        var filteredProducts = products;
        if (options.MaximumPrice != null)
            filteredProducts = filteredProducts.Where(p => p.Price <= options.MaximumPrice);
        
        if (options.MinimumPrice != null)
            filteredProducts = filteredProducts.Where(p => p.Price >= options.MinimumPrice);
        
        return filteredProducts;
    }

    private static IEnumerable<Product> ApplySort(this IEnumerable<Product> products, ProductQueryOptions options)
    {
        var sortedProducts = products;
        sortedProducts = sortedProducts.SortCollection(p => p.Name, "name", options);
        sortedProducts = sortedProducts.SortCollection(p => p.Price, "price", options);
        return sortedProducts;
    }
}
