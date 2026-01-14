namespace spark.Dtos;

/// <summary>
/// DTO used when registering a new user.
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// Email address for the new user.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Password for the new user.
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary>
    /// First name of the user.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Last name of the user.
    /// </summary>
    public string LastName { get; set; } = string.Empty;
}
