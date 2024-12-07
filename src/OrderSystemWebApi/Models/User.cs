using System;
using Microsoft.AspNetCore.Identity;

namespace OrderSystemWebApi.Models;

public class User : IdentityUser
{
    public string FirstName {get; set;} = null!;
    public string LastName {get; set;} = null!;

    public ICollection<Order> Orders = [];
}
