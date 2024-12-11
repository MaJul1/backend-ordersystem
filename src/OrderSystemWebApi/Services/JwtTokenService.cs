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
    private readonly ILogger<JwtTokenService> _logger;
    public JwtTokenService (IConfiguration configuration, IUserRepositoryService userService, ILogger<JwtTokenService> logger)
    {
        _configuration = configuration;
        _userService = userService;
        _logger = logger;
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

    public async Task<string> GetIdAsync(string Token)
    {
        _logger.LogDebug("GetIdAsync(): Request received: Token = start->{token}<-end", Token);
        var handler = new JwtSecurityTokenHandler();

        var result = await handler.ValidateTokenAsync(Token, GetTokenValidationParameters());
        
        _logger.LogDebug("GetIdAsync(): The token validity result is {IsValid}", result.IsValid);
        if (result.IsValid == false)
            throw new ArgumentException("Token is invalid.");
        
        var id = result.Claims.FirstOrDefault(c => c.Key == ClaimTypes.NameIdentifier).Value;
        return (string)id;
    }

    private TokenValidationParameters GetTokenValidationParameters()
    {
        var parameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = _configuration["Jwt:Audience"],
            ValidIssuer = _configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!))
        };

        return parameters;
    }
}
