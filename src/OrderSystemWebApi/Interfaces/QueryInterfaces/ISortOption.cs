using System;

namespace OrderSystemWebApi.Interfaces.QueryInterfaces;

public interface ISortOption
{
    string? OrderByPropertyName {get; set;}
    bool IsDescending {get; set;}
}
