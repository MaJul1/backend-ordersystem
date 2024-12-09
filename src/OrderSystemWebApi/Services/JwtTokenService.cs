using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OrderSystemWebApi.Interfaces;
using OrderSystemWebApi.Models;

namespace OrderSystemWebApi.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepositoryService _userService;
    public JwtTokenService (IConfiguration configuration, IUserRepositoryService userService)
    {
        _configuration = configuration;
        _userService = userService;
    }

    public async Task<string> GenerateToken(User user)
    {
        var handler = new JwtSecurityTokenHandler();
        var key =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
        var roles = await _userService.GetRoles(user);
        var claims = new ClaimsIdentity();
        
        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));

        foreach (var role in roles)
        {
            claims.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        var token = handler.CreateJwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            subject: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return handler.WriteToken(token);

    }
}
