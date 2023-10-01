using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static PropertyBuildingDemo.Domain.Common.Constants;

namespace PropertyBuildingDemo.Application.Dto
{
    /// <summary>
    /// Data transfer object (DTO) for registering a new user.
    /// </summary>
    public class UserRegisterDto
    {
        /// <summary>
        /// <summary>
        /// Gets or sets the identification number for the user. For this sample 
        /// </summary>
        [Required]
        [Range(AppUserConstats.IDENTITY_NUMBER_VALUE_MIN, AppUserConstats.IDENTITY_NUMBER_VALUE_MAX, ErrorMessage = "The value must be within the valid range.")]
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
        [RegularExpression("(?=^.{5,25}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$",
            ErrorMessage = "Password Must have At least 1 Lowercase, 1 Uppercase, and 1 Special Character")]
        public string Password { get; set; }
    }
}
