using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PropertyBuildingDemo.Application.Dto
{
    /// <summary>
    /// Data transfer object (DTO) for registering a new user.
    /// </summary>
    public class UserRegisterDto
    {
        /// <summary>
        /// <summary>
        /// Gets or sets the identification number for the user.
        /// </summary>
        [Required]
        public int IdentificationNumber { get; set; }

        /// <summary>
        /// Gets or sets the display name for the user.
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the email address for the user.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password for the user.
        /// </summary>
        [PasswordPropertyText]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$",
            ErrorMessage = "Password Must have At least 1 Lowercase, 1 Uppercase, and 1 Special Character")]
        public string Password { get; set; }
    }
}
