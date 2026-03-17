using System.ComponentModel.DataAnnotations;

namespace Clothify.Application.DTOs.Request;

public class RegisterRequest
{
    [Required, MinLength(2)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MinLength(2)]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required, MinLength(8)]
    public string Password { get; set; } = string.Empty;

    [Required, Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;
}
