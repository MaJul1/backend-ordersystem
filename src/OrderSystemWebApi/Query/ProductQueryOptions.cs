using System;
using System.ComponentModel.DataAnnotations;
using OrderSystemWebApi.Interfaces.QueryInterfaces;

namespace OrderSystemWebApi.Query;

public class ProductQueryOptions : IPriceFilterOption, ISortOption, IPaginationOption
{
    public double? MinimumPrice { get; set; }
    public double? MaximumPrice { get; set; }
    public string? OrderByPropertyName { get; set; }
    public bool IsDescending { get; set; } = false;
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
}
