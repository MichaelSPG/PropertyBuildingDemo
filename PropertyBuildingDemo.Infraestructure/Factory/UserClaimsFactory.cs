using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PropertyBuildingDemo.Domain.Entities.Identity;
using System.Security.Claims;

namespace PropertyBuildingDemo.Infrastructure.Factory
{
    /// <summary>
    /// Factory for generating claims for a user.
    /// </summary>
    public class UserClaimsFactory : UserClaimsPrincipalFactory<AppUser, IdentityRole>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserClaimsFactory"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="roleManager">The role manager.</param>
        /// <param name="optionsAccessor">The options accessor.</param>
        public UserClaimsFactory(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        /// <summary>
        /// Generates claims for the user.
        /// </summary>
        /// <param name="user">The user for which claims are generated.</param>
        /// <returns>A <see cref="ClaimsIdentity"/> containing the generated claims.</returns>
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("id", user.Id));
            identity.AddClaim(new Claim("fullName", user.DisplayName));
            return identity;
        }
    }
}
