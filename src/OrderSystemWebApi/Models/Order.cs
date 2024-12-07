using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderSystemWebApi.Models;

public class Order
{
    public Guid Id {get; set;}
    public string UserId {get; set;} = null!;

    public User User {get; set;} = new();
    public ICollection<Product> Products {get; set;} = [];
}
