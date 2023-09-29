namespace PropertyBuildingDemo.Application.Dto
{
    /// <summary>
    /// Data transfer object (DTO) for user login.
    /// </summary>
    public class UserLoginDto
    {
        /// <summary>
        /// Gets or sets the email address for user login.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password for user login.
        /// </summary>
        public string Password { get; set; }
    }
}
