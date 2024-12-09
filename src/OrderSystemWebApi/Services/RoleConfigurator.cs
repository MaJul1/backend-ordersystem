using System;
using Microsoft.AspNetCore.Identity;

namespace OrderSystemWebApi.Services;

public static class RoleConfigurator
{
    public static async Task UseRoles(this WebApplication app)
    {
        string[] roles = ["User", "Moderator", "Admin"];
        
        using var scope = app.Services.CreateScope();

        var rolemanager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
        
        foreach (var role in roles)
        {
            await AddRolesIfNotExists(role, rolemanager);
        }
    }

    private static async Task AddRolesIfNotExists(string role, RoleManager<IdentityRole>? rolemanager)
    {
        if (await rolemanager!.RoleExistsAsync(role) == false)
        {
            await rolemanager.CreateAsync(new IdentityRole(role));
        }
    }
}
