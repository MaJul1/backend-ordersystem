using System;
using Serilog;
using Serilog.Configuration;
using Serilog.Filters;
namespace OrderSystemWebApi.Services;

public static class SerilogConfigurator
{
    public static void ConfigureSerilog(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
        
        builder.Host.UseSerilog();
    }
}
