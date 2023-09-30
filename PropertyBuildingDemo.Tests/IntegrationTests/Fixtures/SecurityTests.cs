using Microsoft.AspNetCore.Mvc.Testing;
using PropertyBuildingDemo.Api;
using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Tests.Helpers;
using System.Net;
using System.Net.Http.Json;
using PropertyBuildingDemo.Domain.Entities.Identity;
using PropertyBuildingDemo.Tests.Factories;

namespace PropertyBuildingDemo.Tests.IntegrationTests.Fixtures
{
    public class SecurityTests : IDisposable
    {
        private HttpClient _client;
        private TestWebApplicationFactory<Program> _factory;
        private TokenResponse _tokenResponse;
        private UserRegisterDto _userRegisterDto;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _factory = new TestWebApplicationFactory<Program>();
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        }

        [SetUp]
        public void Setup()
        {
            _client = _factory.CreateClient();
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
            var response = await _client.GetAsync($"{url}");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test(Description = "Test user account creation specifying bad password, expected to return BadRequest")]
        public async Task Should_ReturnBadRequestResponse_When_AnonymousUserCreatesUserWithInvalidPassword()
        {
            _userRegisterDto = AccountUserDataFactory.CreateInvalidUserPasswordFoRegister();

            var response = await _client.PostAsJsonAsync($"{TestConstants.AccountEnpoint.Register}", _userRegisterDto);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest),
                $"Status code must be {HttpStatusCode.BadRequest}");
        }

        [Test]
        public async Task Should_ReturnOkResponseWithUserData_When_RegisterAccountUser()
        {
            // Arrange: Prepare the user data for creation
            _userRegisterDto = AccountUserDataFactory.CreateValidUserFoRegister();

            // Act: Make the HTTP request to create an anonymous user
            var response = await _client.PostAsJsonAsync($"{TestConstants.AccountEnpoint.Register}", _userRegisterDto);

            // Deserialize the response content to a strongly typed object
            ApiResult<UserDto> result = await response.Content.ReadFromJsonAsync<ApiResult<UserDto>>();

            // Assert: Verify the response
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), $"Status code must be {HttpStatusCode.OK}");

            // Validate the API result data using a utility method
            Utilities.ValidateApiResultData(result);

            // Assert that the User Id is not null or empty
            Assert.IsTrue(!string.IsNullOrWhiteSpace(result.Data.Id), $"User Id must not be null/empty");
        }


        [Test()]
        public async Task Should_ReturnUserNotFoundResponse_When_UserRequestTokenWithNullData()
        {
            TokenRequest request = null;

            var response = await _client.PostAsJsonAsync($"{TestConstants.AccountEnpoint.Login}", request);

            Assert.That(response.StatusCode, Is.Not.EqualTo(HttpStatusCode.OK),
                $"Status code must be different of {HttpStatusCode.OK}");
        }

        [Test()]
        public async Task Should_ReturnUnauthorizedResponse_When_UsingExpiredToken()
        {
            _client.DefaultRequestHeaders.Add("access_token", TokenDataFactory.CreateExpiredTokenResponse().Token);
            var response = await _client.GetAsync($"{TestConstants.AccountEnpoint.CurrentUser}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized),
                $"Response must be {HttpStatusCode.Unauthorized}");
        }
        [Test]
        public async Task Should_ReturnUnauthorizedResponse_When_UsingInvalidTokenData()
        {
            // Act: Make the HTTP request with an invalid access token
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer someInvalidData");
            var response = await _client.GetAsync($"{TestConstants.AccountEnpoint.CurrentUser}");

            // Assert: Verify the response
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode, "Response status code should be Unauthorized (401)");
        }

        [Test()]
        public async Task Should_ReturnUnauthorizedResponse_When_UserRequestTokenWithIncorrectPassword()
        {
            TokenRequest request =
                TokenDataFactory.CreateTokenRequestCustom(AccountUserDataFactory.CreateValidUserFoRegister().Email,
                    "NotValidPaswword");
            var response = await _client.PostAsJsonAsync($"{TestConstants.AccountEnpoint.Login}", request);
            ApiResult<TokenResponse> result = await response.Content.ReadFromJsonAsync<ApiResult<TokenResponse>>();

            Assert.That(response.StatusCode, Is.Not.EqualTo(HttpStatusCode.OK),
                $"Status code must be different of {HttpStatusCode.OK}");

            Utilities.ValidateApiResult_ExpectedNotOk(result);
        }

        [Test()]
        public async Task Should_ReturnAuthorizedResponse_When_ValidatingUserToken()
        {
            Assert.IsNotNull(_tokenResponse);
            _client.DefaultRequestHeaders.Add("access_token", _tokenResponse.Token);
            var response = await _client.GetAsync($"{TestConstants.AccountEnpoint.CurrentUser}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), $"Response must be {HttpStatusCode.OK}");
            ApiResult<UserDto> result = await response.Content.ReadFromJsonAsync<ApiResult<UserDto>>();
            Utilities.ValidateApiResultData(result);
        }
        [Test()]
        public async Task Should_ReturnOkResponseWithTokenData_When_RequestingUserToken()
        {
            var validUserRegistration = AccountUserDataFactory.CreateValidUserFoRegister();

            TokenRequest request =
                TokenDataFactory.CreateTokenRequestCustom(validUserRegistration.Email, validUserRegistration.Password);

            var response = await _client.PostAsJsonAsync($"{TestConstants.AccountEnpoint.Login}", request);

            ApiResult<TokenResponse> result = await response.Content.ReadFromJsonAsync<ApiResult<TokenResponse>>();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), $"Status code must be {HttpStatusCode.OK}");

            Utilities.ValidateApiResultData(result);

            _tokenResponse = result.Data;

            Assert.IsFalse(string.IsNullOrWhiteSpace(_tokenResponse.Token), $"Token must not be null/empty");

            Assert.Less(DateTime.UtcNow, _tokenResponse.TokenExpiryTime, $"Token expiration time must be greater than actual date/time");
        }

    }
}