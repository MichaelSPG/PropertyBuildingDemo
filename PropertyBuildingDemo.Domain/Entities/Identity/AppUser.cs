using Microsoft.AspNetCore.Identity;
using PropertyBuildingDemo.Domain.Entities.Enums;

namespace PropertyBuildingDemo.Domain.Entities.Identity
{
    /// <summary>
    /// Represents an application user.
    /// </summary>
    public class AppUser : IdentityUser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppUser"/> class.
        /// </summary>
        public AppUser()
        {
            this.UserName = String.Empty;
        }

        /// <summary>
        /// Gets or sets the display name of the user.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the role of the user.
        /// </summary>
        public ERoleType Role { get; set; }

        /// <summary>
        /// Gets or sets the identity number of the user.
        /// </summary>
        public int IdentificationNumber { get; set; }

        /// <summary>
        /// Gets or sets the refresh token for the user.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the expiry time of the refresh token.
        /// </summary>
        public DateTime? RefreshTokenExpiryTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is active.
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
