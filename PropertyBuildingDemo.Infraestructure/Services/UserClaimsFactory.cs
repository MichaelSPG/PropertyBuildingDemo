using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using PropertyBuildingDemo.Domain.Entities.Identity;

namespace PropertyBuildingDemo.Infrastructure.Services
{
    public class UserClaimsFactory : UserClaimsPrincipalFactory<AppUser, IdentityRole>
    {
        public UserClaimsFactory(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("id", user.Id));
            identity.AddClaim(new Claim("fullName", user.DisplayName));
            return identity;
        }
    }
}
