using OrderSystemWebApi.Interfaces.QueryInterfaces;

namespace OrderSystemWebApi.Query;

public static class PaginationService
{
    public static IEnumerable<T> ApplyPagination<T>(this IEnumerable<T> collection, IPaginationOption option)
    {
        if (option.PageNumber == null || option.PageSize == null)
            return collection;
        
        int skip = (int)((option.PageNumber - 1) * option.PageSize);
        int take = (int)option.PageSize;

        return collection.Skip(skip).Take(take);
    }
}
