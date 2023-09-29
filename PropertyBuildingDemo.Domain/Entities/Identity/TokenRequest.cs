using System.ComponentModel.DataAnnotations;

namespace PropertyBuildingDemo.Domain.Entities.Identity
{
    /// <summary>
    /// Represents a request to obtain an authentication token.
    /// </summary>
    public class TokenRequest
    {
        /// <summary>
        /// Gets or sets the username for authentication.
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password for authentication.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
