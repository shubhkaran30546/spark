namespace spark.Dtos
{
    /// <summary>
    /// DTO used for user login requests.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// User email used for login.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// User password used for login.
        /// </summary>
        public string Password { get; set; } = null!;
    }   
}