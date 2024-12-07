using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using OrderSystemWebApi.Models;

namespace OrderSystemWebApi.Context;

public class OrderSystemContext : IdentityDbContext<User, IdentityRole, string>
{
    public OrderSystemContext (DbContextOptions options) : base (options)
    {}

    public DbSet<Product> Products {get; set;}
    public DbSet<Order> Orders {get; set;}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(o => o.Orders)
            .HasForeignKey(o => o.UserId);

        builder.Entity<Product>()
            .HasMany(o => o.Orders)
            .WithMany(o => o.Products)
            .UsingEntity(o => o.ToTable("OrderProducts"));
    }
}
