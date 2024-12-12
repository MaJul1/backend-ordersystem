using System;
using OrderSystemWebApi.DTO.Product;

namespace OrderSystemWebApi.DTO.Order;

public class ReadOrderRequestDTO
{
    public Guid Id {get; set;}
    public Guid Buyer {get; set;}
    public ICollection<ReadProductRequestDTO> ProductsOrdered {get; set;} = null!;
}
