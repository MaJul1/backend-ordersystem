using System;
using OrderSystemWebApi.Models;

namespace OrderSystemWebApi.Interfaces;

public interface IJwtTokenService
{
    Task<string> GenerateToken(User user);
}
