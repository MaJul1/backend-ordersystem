using OrderSystemWebApi.Interfaces.QueryInterfaces;

namespace OrderSystemWebApi.Query.Service;

public static class SortService
{
    public static IEnumerable <T> SortCollection<T> (this IEnumerable<T> collection, Func<T, object> func, string TargetPropertyName, ISortOption option)
    {
        if (option.OrderByPropertyName == null)
            return collection;
        
        if(option.OrderByPropertyName.Equals(TargetPropertyName, StringComparison.OrdinalIgnoreCase) == false)
            return collection;

        return option.IsDescending ? OrderByDescending(collection, func) : OrderByAscending(collection, func);
    }

    private static IEnumerable<T> OrderByDescending<T> (IEnumerable<T> collection, Func<T, object> func)
    {
        return collection.OrderByDescending(func);
    }

    private static IEnumerable<T> OrderByAscending<T> (IEnumerable<T> collection, Func<T, object> func)
    {
        return collection.OrderBy(func);
    }
}
