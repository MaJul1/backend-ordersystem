using System;

namespace OrderSystemWebApi.Models;

public class Product
{
    public Guid Id {get; set;}
    public string Name {get; set;} = null!;
    public double Price {get; set;}

    public ICollection<Order> Orders {get; set;} = [];
}
