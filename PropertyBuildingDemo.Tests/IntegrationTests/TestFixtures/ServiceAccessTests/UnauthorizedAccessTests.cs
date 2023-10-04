using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Entities.Identity;
using PropertyBuildingDemo.Tests.Factories;
using PropertyBuildingDemo.Tests.Helpers;
using System.Net;
using PropertyBuildingDemo.Tests.Helpers.Config;

namespace PropertyBuildingDemo.Tests.IntegrationTests.TestFixtures.ServiceAccessTests
{
    /// <summary>
    /// Represents a test fixture for unauthorized access scenarios.
    /// </summary>
    [TestFixture]
    public class UnauthorizedAccessTests : BaseTest
    {
        /// <summary>
        /// Test to verify that a bad request response is returned when a user logs in with null data.
        /// </summary>
        [Test]
        public async Task Should_ReturnBadRequestResponse_When_UserLoginsWithNullData()
        {
            await HttpApiClient.MakeApiPostRequestAsync<TokenResponse>($"{AccountEndpoint.Login}", Is.EqualTo(HttpStatusCode.BadRequest));
        }

        /// <summary>
        /// Test to verify that an unauthorized response is returned when using an expired token.
        /// </summary>
        [Test]
        public async Task Should_ReturnUnauthorizedResponse_When_UsingExpiredToken()
        {
            // Arrange: Set the authorization header with an expired token
            await HttpApiClient.SetTokenAuthorizationHeader(TokenDataFactory.CreateExpiredTokenResponse());

            // Act: Make the HTTP request with the expired token
            await HttpApiClient.MakeApiGetRequestAsync<TokenResponse>($"{AccountEndpoint.CurrentUser}", Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        /// <summary>
        /// Test to verify that an unauthorized response is returned when using invalid token data.
        /// </summary>
        [Test]
        public async Task Should_ReturnUnauthorizedResponse_When_UsingInvalidTokenData()
        {
            // Act: Make the HTTP request with an invalid access token
            await HttpApiClient.SetTokenAuthorizationHeader(TokenDataFactory.CreateCorruptedTokenResponse());
            await HttpApiClient.MakeApiGetRequestAsync<TokenResponse>($"{AccountEndpoint.CurrentUser}", Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}