using System.ComponentModel.DataAnnotations;

namespace AccountService.Requests;

public class CreateAccountRequest
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
}
