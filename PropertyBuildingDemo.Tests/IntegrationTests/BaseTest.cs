using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using PropertyBuildingDemo.Domain.Entities.Identity;
using PropertyBuildingDemo.Tests.IntegrationTests.TestUtilities;
using System.Net.Http.Headers;

namespace PropertyBuildingDemo.Tests.IntegrationTests
{
    public class BaseTest : IDisposable
    {
        protected HttpApiClient ApiClient;
        protected ApiWebApplicationFactory Application;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Application = new ApiWebApplicationFactory();
            ApiClient = new HttpApiClient(Application.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false }));
        }
        public void Dispose()
        {
            ApiClient?.Dispose();
            Application?.Dispose();
        }


        //public async Task<(HttpClient Client, string UserId)> CreateTestUser(string userName, string password, string[] roles)
        //{
        //    using var scope = Application.Services.CreateScope();
        //    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

        //    var newUser = new AppUser();
        //    newUser.UserName = userName;
        //    newUser.DisplayName = userName;
        //    await userManager.CreateAsync(newUser, password);

        //    foreach (var role in roles)
        //    {
        //        await userManager.AddToRoleAsync(newUser, role);
        //    }

        //    var accessToken = await GetAccessToken(userName, password);

        //    var client = Application.CreateClient();
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        //    return (client, newUser.Id);
        //}
    }
}
