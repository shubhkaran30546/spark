using System.ComponentModel.DataAnnotations;

/// <summary>
/// Model used when a user requests a password recovery or reset.
/// </summary>
public class RecoverPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// Optional security question displayed to the user.
    /// </summary>
    public string SecurityQuestion { get; set; }

    [Required]
    public string SecurityAnswer { get; set; }

    [Required]
    public string NewPassword { get; set; }
}