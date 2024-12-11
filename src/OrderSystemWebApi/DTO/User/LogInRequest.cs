using System;
using System.ComponentModel.DataAnnotations;

namespace OrderSystemWebApi.DTO.User;

public class LogInRequest
{
    [Required]
    public string Username {get; set;} = null!;

    [Required]
    public string Password {get; set;} = null!;
}
