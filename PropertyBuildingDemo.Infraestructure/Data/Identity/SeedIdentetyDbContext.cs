using Microsoft.AspNetCore.Identity;
using PropertyBuildingDemo.Domain.Entities.Enums;
using PropertyBuildingDemo.Domain.Entities.Identity;

namespace PropertyBuildingDemo.Infrastructure.Data.Identity
{
    /// <summary>
    /// Class responsible for seeding user data in the Identity database.
    /// </summary>
    public class SeedIdentityDbContext
    {
        /// <summary>
        /// Seed user data into the Identity database using the provided <see cref="UserManager{TUser}"/>.
        /// </summary>
        /// <param name="userManager">The UserManager for managing user accounts.</param>
        /// <returns>An asynchronous task representing the operation.</returns>
        public static async Task SeedUserData(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                // Create a new user with the specified data
                AppUser appUser = new AppUser
                {
                    DisplayName = "Christopher",
                    UserName = "christopher92@stwnet.com",
                    Email = "christopher92@stwnet.com",
                    Role = ERoleType.Admin,
                    IdentificationNumber = 12359233
                };

                // Attempt to create the user with a password
                var result = await userManager.CreateAsync(appUser, "@1234#");
            }
        }
    }
}
