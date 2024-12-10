using System;
using Microsoft.AspNetCore.Identity;
using OrderSystemWebApi.DTO;
using OrderSystemWebApi.Models;

namespace OrderSystemWebApi.Mapper;

public static class UserMapper
{   
    public static User ToUser(this WriteUserRequestDTO request)
    {
        User user = new()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.Username,
        };

        return user;
    }

    public static LogInResponse ToLogInResponse(this User user, string token)
    {
        var readUser = new LogInResponse()
        {
            Token = token,
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Username = user.UserName!
        };

        return readUser;
    }
}
