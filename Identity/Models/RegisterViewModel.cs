using System.ComponentModel.DataAnnotations;

namespace Flora.Identity.Models;

public class RegisterViewModel
{
    [Required] public string Email { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = null!;

    [Required(ErrorMessage = "Confirm Password is required")]
    public string Role { get; set; } = null!;

    public string? ReturnUrl { get; set; }
}