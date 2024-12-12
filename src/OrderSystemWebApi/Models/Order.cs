using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OrderSystemWebApi.Models;

public class Order
{
    public Guid Id {get; set;}
    public string UserId {get; set;} = null!;
    public User User {get; set;} = null!;
    
    public ICollection<Product> Products {get; set;} = [];
}
