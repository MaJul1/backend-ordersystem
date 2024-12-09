using System;
using System.ComponentModel.DataAnnotations;

namespace OrderSystemWebApi.DTO;

public class WriteUserRequestDTO
{
    [Required(ErrorMessage = "First name is required.")]
    [Length(3, 120, ErrorMessage = "First name must be betweenn 3 and 120 characters.")]
    public string FirstName {get; set;} = null!;

    [Required(ErrorMessage = "Last name is required.")]
    [Length(3, 120, ErrorMessage = "Last name must be betweenn 3 and 120 characters.")]
    public string LastName {get; set;} = null!;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email must be valid.")]
    public string Email {get; set;} = null!;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Password must contain at least one uppercase letter and one digit.")]
    public string Password {get; set;} = null!;
}
