using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Entities.Identity;
using PropertyBuildingDemo.Tests.Factories;
using PropertyBuildingDemo.Tests.Helpers;
using System.Net;

namespace PropertyBuildingDemo.Tests.IntegrationTests.TestFixtures.ServiceAccessTests
{
    [TestFixture]
    public class UnauthorizedAccessTests : BaseTest
    {
        private UserRegisterDto _userRegisterDto;

        // SECURITY TESTS

        //[Test(Description = "Test if we don't have access to any endpoint")]
        //[TestCase(TestConstants.OwnerEnpoint.List)]
        //[TestCase(TestConstants.PropertyImageEnpoint.List)]
        //[TestCase(TestConstants.PropertyBuildingEnpoint.ListBy)]
        //[TestCase(TestConstants.AccountEnpoint.CurrentUser)]
        //public async Task Should_ReturnUnauthorizedResponse_When_AnonymousUserConnectsToService(string url)
        //{
        //    await httpApiClient.MakeApiGetRequestAsync<object>($"{url}", Is.EqualTo(HttpStatusCode.Unauthorized));
        //}

        [Test()]
        public async Task Should_ReturnBadRequestResponse_When_UserLoginsWithNullData()
        {
            await HttpApiClient.MakeApiPostRequestAsync<TokenResponse>($"{TestConstants.AccountEndpoint.Login}",  Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test()]
        public async Task Should_ReturnUnauthorizedResponse_When_UsingExpiredToken()
        {
            await HttpApiClient.SetTokenAuthorizationHeader(TokenDataFactory.CreateExpiredTokenResponse());
            await HttpApiClient.MakeApiGetRequestAsync<TokenResponse>($"{TestConstants.AccountEndpoint.CurrentUser}", Is.EqualTo(HttpStatusCode.Unauthorized));
        }
        [Test]
        public async Task Should_ReturnUnauthorizedResponse_When_UsingInvalidTokenData()
        {
            // Act: Make the HTTP request with an invalid access token
            await HttpApiClient.SetTokenAuthorizationHeader(TokenDataFactory.CreateCorruptedTokenResponse());
            await HttpApiClient.MakeApiGetRequestAsync<TokenResponse>($"{TestConstants.AccountEndpoint.CurrentUser}", Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}