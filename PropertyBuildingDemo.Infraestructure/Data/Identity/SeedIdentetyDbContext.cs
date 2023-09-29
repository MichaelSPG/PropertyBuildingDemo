using Microsoft.AspNetCore.Identity;
using PropertyBuildingDemo.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyBuildingDemo.Domain.Entities.Enums;
using static Duende.IdentityServer.Models.IdentityResources;

namespace PropertyBuildingDemo.Infrastructure.Data.Identity
{
    public  class SeedIdentetyDbContext
    {
        public static async Task SeedUserData(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                AppUser appUser = new AppUser
                {
                    DisplayName = "Christopher",
                    UserName = "christopher92@stwnet.com",
                    Email = "christopher92@stwnet.com",
                    Role = ERoleType.RoleAdmin,
                    IdentityNumber = "12359233"
                };
                var result = await userManager.CreateAsync(appUser, "@1234#");
            }
        }
    }
}
