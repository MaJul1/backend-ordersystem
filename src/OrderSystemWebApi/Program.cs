using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Identity;
using OrderSystemWebApi.Models;
using OrderSystemWebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.ConfigureDatabase();
builder.ConfigureIdentity();
builder.ConfigureAppSettingJsons();
builder.ConfigureCustomServices();
builder.ConfigureJwtAuthentication();

builder.Services.AddAuthorization();
builder.Services.AddLogging();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

await app.UseRoles();

app.Run();
