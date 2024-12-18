using System;
using OrderSystemWebApi.Interfaces;

namespace OrderSystemWebApi.Services;

public class LoggerService : ILoggerService
{
    private readonly ILogger _logger;
    private readonly IJwtTokenService _jwt;
    private readonly IControllerServices _controller;
    public LoggerService(ILogger<LoggerService> logger, IJwtTokenService jwt, IControllerServices controller)
    {
        _logger = logger;
        _jwt = jwt;
        _controller = controller;
        
    }

    public async Task LogRequestInformation(HttpRequest request, string message)
    {
        var id = await _controller.GetUserIdFromAuthorizationHeaderAsync(request);

        _logger.LogInformation($"{id} {message}");
    }
}
