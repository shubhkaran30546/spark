using System.ComponentModel.DataAnnotations;

public class RecoverPasswordViewModel
{

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string SecurityQuestion { get; set; }
    [Required]
    public string SecurityAnswer { get; set; }

    [Required]
    public string NewPassword { get; set; }
}