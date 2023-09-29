using Microsoft.AspNetCore.Identity;
using PropertyBuildingDemo.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Application.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<AppUser> FindByEmailFromClaimPrincipal(
            this UserManager<AppUser> input, ClaimsPrincipal user)
        {
            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            return await input.Users.SingleOrDefaultAsync(x => x.Email == email);
        }
    }
}
