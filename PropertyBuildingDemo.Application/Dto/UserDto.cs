namespace PropertyBuildingDemo.Application.Dto
{
    /// <summary>
    /// Data transfer object (DTO) for user information.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Get or sets Id of this user
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the identity number of the user.
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        /// Gets or sets the display name of the user.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; }
    }
}
