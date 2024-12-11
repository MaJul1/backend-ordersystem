using System;
using Microsoft.IdentityModel.Tokens;
using OrderSystemWebApi.Models;

namespace OrderSystemWebApi.Interfaces;

public interface IJwtTokenService
{
    Task<string> GenerateToken(User user);
    Task<string> GetIdAsync(string Token);
}
