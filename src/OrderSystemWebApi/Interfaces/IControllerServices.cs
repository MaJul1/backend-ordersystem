using System;

namespace OrderSystemWebApi.Interfaces;

public interface IControllerServices
{
    Task<string> GetUserIdFromAuthorizationHeaderAsync(HttpRequest request);
}
