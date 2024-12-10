using System;

namespace OrderSystemWebApi.DTO;

public class LogInResponse
{
    public string Id {get; set;} = null!;
    public string Username {get; set;} = null!;
    public string FirstName {get; set;} = null!;
    public string LastName {get; set;} = null!;
    public string Token {get; set;} = null!;

}
