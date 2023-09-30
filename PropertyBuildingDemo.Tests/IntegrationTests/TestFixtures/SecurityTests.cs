using Microsoft.AspNetCore.Mvc.Testing;
using PropertyBuildingDemo.Api;
using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Tests.Helpers;
using System.Net;
using System.Net.Http.Json;
using PropertyBuildingDemo.Domain.Entities.Identity;
using PropertyBuildingDemo.Tests.Factories;
using PropertyBuildingDemo.Tests.IntegrationTests.TestUtilities;

namespace PropertyBuildingDemo.Tests.IntegrationTests.TestFixtures
{
    public class SecurityTests : IDisposable
    {
        private HttpApiClient _client;
        private TestWebApplicationFactory<Program> _factory;
        private TokenResponse _tokenResponse;
        private UserRegisterDto _userRegisterDto;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _factory = new TestWebApplicationFactory<Program>();
            _client = new HttpApiClient(_factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false }));
        }

        [SetUp]
        public void Setup()
        {
        }

        public void Dispose()
        {
            _client?.Dispose();
            _factory?.Dispose();
        }

        // SECURITY TESTS

        [Test(Description = "Test if we don't have access to any endpoint")]
        [TestCase(TestConstants.OwnerEnpoint.List)]
        [TestCase(TestConstants.PropertyEnpoint.List)]
        [TestCase(TestConstants.PropertyImageEnpoint.List)]
        [TestCase(TestConstants.PropertyBuildingEnpoint.ListBy)]
        [TestCase(TestConstants.AccountEnpoint.CurrentUser)]
        public async Task Should_ReturnUnauthorizedResponse_When_AnonymousUserConnectsToService(string url)
        {
            await _client.MakeGetRequestAsync<object>($"{url}", Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test(Description = "Test user account creation specifying bad password, expected to return BadRequest")]
        public async Task Should_ReturnBadRequestResponse_When_AnonymousUserCreatesUserWithInvalidPassword()
        {
            _userRegisterDto = AccountUserDataFactory.CreateInvalidUserPasswordFoRegister();
            await _client.MakePostRequestAsync<object> ($"{TestConstants.AccountEnpoint.Register}", _userRegisterDto, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Should_ReturnOkResponseWithUserData_When_RegisterAccountUser()
        {
            // Arrange: Prepare the user data for creation
            _userRegisterDto = AccountUserDataFactory.CreateValidUserFoRegister();

            // Act: Make the HTTP request to create an anonymous user
            var result = await _client.MakePostRequestAsync<UserDto>($"{TestConstants.AccountEnpoint.Register}", _userRegisterDto, Is.EqualTo(HttpStatusCode.OK));

            // Validate the API result data using a utility method
            Utilities.ValidateApiResultData(result);

            // Assert that the User Id is not null or empty
            Assert.IsTrue(!string.IsNullOrWhiteSpace(result.Data.Id), $"User Id must not be null/empty");
        }


        [Test()]
        public async Task Should_ReturnBadRequestResponse_When_UserRequestTokenWithNullData()
        {
            await _client.MakePostRequestAsync<TokenResponse>($"{TestConstants.AccountEnpoint.Login}", null, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test()]
        public async Task Should_ReturnUnauthorizedResponse_When_UsingExpiredToken()
        {
            await _client.SetTokenAuthorizationHeader(TokenDataFactory.CreateExpiredTokenResponse());
            await _client.MakeGetRequestAsync<TokenResponse>($"{TestConstants.AccountEnpoint.CurrentUser}", Is.EqualTo(HttpStatusCode.Unauthorized));
        }
        [Test]
        public async Task Should_ReturnUnauthorizedResponse_When_UsingInvalidTokenData()
        {
            // Act: Make the HTTP request with an invalid access token
            await _client.SetTokenAuthorizationHeader(TokenDataFactory.CreateCorruptedTokenResponse());
            await _client.MakeGetRequestAsync<TokenResponse>($"{TestConstants.AccountEnpoint.CurrentUser}", Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test()]
        public async Task Should_ReturnUnauthorizedResponse_When_UserRequestTokenWithIncorrectPassword()
        {
            TokenRequest request = TokenDataFactory.CreateTokenRequestCustom(AccountUserDataFactory.CreateValidUserFoRegister().Email,
                    "NotValidPassword");

            var result = await _client.MakePostRequestAsync<TokenResponse>($"{TestConstants.AccountEnpoint.Login}", request, Is.EqualTo(HttpStatusCode.OK));

            Utilities.ValidateApiResult_ExpectedNotOk(result);
            Assert.IsTrue(
                result.GetJoinedMessages().IndexOf("invalid credentials", StringComparison.OrdinalIgnoreCase) >= 0,
                $"Result message must contain 'password'. Actual message: {result.GetJoinedMessages()}"
            );
        }

        //[Test()]
        //public async Task Should_ReturnAuthorizedResponse_When_ValidatingUserToken()
        //{

        //    _client.DefaultRequestHeaders.Add("access_token", _tokenResponse.Token);

        //    await _client.SetTokenAuthorizationHeader(TokenDataFactory.CreateCorruptedTokenResponse());

        //    var response = await _client.GetAsync($"{TestConstants.AccountEnpoint.CurrentUser}");

        //    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), $"Response must be {HttpStatusCode.OK}");
        //    ApiResult<UserDto> result = await response.Content.ReadFromJsonAsync<ApiResult<UserDto>>();
        //    Utilities.ValidateApiResultData(result);
        //}
        //[Test()]
        //public async Task Should_ReturnOkResponseWithTokenData_When_RequestingUserToken()
        //{
        //    var validUserRegistration = AccountUserDataFactory.CreateValidUserFoRegister();

        //    TokenRequest request =
        //        TokenDataFactory.CreateTokenRequestCustom(validUserRegistration.Email, validUserRegistration.Password);

        //    var response = await _client.PostAsJsonAsync($"{TestConstants.AccountEnpoint.Login}", request);

        //    ApiResult<TokenResponse> result = await response.Content.ReadFromJsonAsync<ApiResult<TokenResponse>>();

        //    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), $"Status code must be {HttpStatusCode.OK}");

        //    Utilities.ValidateApiResultData(result);

        //    _tokenResponse = result.Data;

        //    Assert.IsFalse(string.IsNullOrWhiteSpace(_tokenResponse.Token), $"Token must not be null/empty");

        //    Assert.Less(DateTime.UtcNow, _tokenResponse.TokenExpiryTime, $"Token expiration time must be greater than actual date/time");
        //}

    }
}