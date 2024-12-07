using System;

namespace OrderSystemWebApi.Services;

public static class AppSettingsConfigurator
{
    public static void ConfigureAppSettingJsons(this WebApplicationBuilder builder)
    {
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsetings{builder.Environment.EnvironmentName}.json", optional: true);
    }
}
