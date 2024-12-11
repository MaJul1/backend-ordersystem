using System;
using Microsoft.AspNetCore.Mvc;
using OrderSystemWebApi.Interfaces;

namespace OrderSystemWebApi.Services;

public class ControllerService : IControllerServices
{
    private readonly IJwtTokenService _jwtService;
    public ControllerService(IJwtTokenService jwtTokenService)
    {
        _jwtService = jwtTokenService;
    }

    public async Task<string> GetUserIdFromAuthorizationHeaderAsync(HttpRequest request)
    {
        if (request.Headers == null || !request.Headers.ContainsKey("Authorization"))
            throw new ArgumentException("Authorization header cannot be found.");

        var authorization = request.Headers.Authorization.FirstOrDefault() ??
            throw new ArgumentNullException(nameof(request), "Authorization header must not be null.");
            
        var token = authorization.Split(" ").Last();

        var userId = await _jwtService.GetIdAsync(token);

        return userId;
    }
}
