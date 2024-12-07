using System;
using Microsoft.EntityFrameworkCore;
using OrderSystemWebApi.Context;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace OrderSystemWebApi.Services;

public static class DatabaseConfigurator
{
    public static void ConfigureDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<OrderSystemContext>(options => 
        {
            options.UseMySql
            (
                builder.Configuration.GetConnectionString("MySqlConnection"), 
                ServerVersion.Create(Version.Parse("8.0.40"), ServerType.MySql)
            );
        });
    }
}
