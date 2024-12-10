using System;
using Microsoft.AspNetCore.Identity;
using OrderSystemWebApi.Context;
using OrderSystemWebApi.Models;

namespace OrderSystemWebApi.Services;

public static class IdentityConfigurator
{
    public static void ConfigureIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<User, IdentityRole>(options =>
        {
            // Password settings
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = false;
        })
        .AddEntityFrameworkStores<OrderSystemContext>()
        .AddDefaultTokenProviders();
    }
}
