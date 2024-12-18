using System;
using OrderSystemWebApi.Services;
using Serilog;

namespace OrderSystemWebApi;

public class Startup
{
    public static async Task InitializeApplication(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();

        builder.ConfigureAppSettingJsons();
        builder.ConfigureSwagger();
        builder.ConfigureDatabase();
        builder.ConfigureIdentity();
        builder.ConfigureCustomServices();
        builder.ConfigureJwtAuthentication();
        builder.ConfigureSerilog();

        builder.Services.AddAuthorization();
        builder.Services.AddLogging();


        var app = builder.Build();

        app.UseSerilogRequestLogging();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
        }

        app.MapControllers();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHttpsRedirection();

        await app.UseRoles();

        app.Run();

    }
}
