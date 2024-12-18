using System;

namespace OrderSystemWebApi.Interfaces;

public interface ILoggerService
{
    Task LogRequestInformation(HttpRequest request, string message);
}
