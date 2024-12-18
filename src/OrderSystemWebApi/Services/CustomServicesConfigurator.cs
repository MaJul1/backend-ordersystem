using System;
using OrderSystemWebApi.Interfaces;
using OrderSystemWebApi.Repository;

namespace OrderSystemWebApi.Services;

public static class CustomServicesConfigurator
{
    public static void ConfigureCustomServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IProductRepositoryService, ProductRepositoryService>();
        builder.Services.AddScoped<IUserRepositoryService, UserRepositoryService>();
        builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
        builder.Services.AddScoped<IOrderRepositoryService, OrderRepositoryService>();
        builder.Services.AddScoped<IControllerServices, ControllerService>();
        builder.Services.AddScoped<IProblemService, ProblemDetailsService>();
        builder.Services.AddScoped<ILoggerService, LoggerService>();
    }
}
