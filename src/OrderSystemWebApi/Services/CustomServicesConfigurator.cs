using System;
using OrderSystemWebApi.Interfaces;
using OrderSystemWebApi.Repository;

namespace OrderSystemWebApi.Services;

public static class CustomServicesConfigurator
{
    public static void ConfigureCustomServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IProductRepositoryService, ProductRepositoryService>();
    }
}
