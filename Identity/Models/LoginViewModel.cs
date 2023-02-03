using System.ComponentModel.DataAnnotations;

namespace Flora.Identity.Models;

public class LoginViewModel
{
    [Required] public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    public string? ReturnUrl { get; set; }
}