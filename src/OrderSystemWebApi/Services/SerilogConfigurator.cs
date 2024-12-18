using System;
using Serilog;
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
