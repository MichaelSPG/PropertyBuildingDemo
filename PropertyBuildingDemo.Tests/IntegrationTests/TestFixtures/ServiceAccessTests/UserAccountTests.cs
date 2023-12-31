﻿using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Entities.Identity;
using PropertyBuildingDemo.Tests.Factories;
using PropertyBuildingDemo.Tests.Helpers;
using System.Net;
using PropertyBuildingDemo.Tests.Helpers.Config;

namespace PropertyBuildingDemo.Tests.IntegrationTests.TestFixtures.ServiceAccessTests
{
    /// <summary>
    /// Represents a test fixture for user account-related scenarios.
    /// </summary>
    public class UserAccountTests : BaseTest
    {
        private UserRegisterDto _validUserRegistration;


        /// <summary>
        /// Set up method to initialize test data.
        /// </summary>
        [SetUp]
        public async Task Setup()
        {
            _validUserRegistration = AccountUserDataFactory.CreateValidTestUserForRegister();
            await SetupUserDataAsync(_validUserRegistration);
        }

        /// <summary>
        /// Provides valid user test cases for registration.
        /// </summary>
        /// <returns>An enumerable of valid user test cases.</returns>
        public static IEnumerable<UserRegisterDto> ValidUserTestCases()
        {
            var list = AccountUserDataFactory.GetValidTestUserForRegisterList(20);
            foreach (var userRegister in list)
            {
                yield return userRegister;
            }
        }
        /// <summary>
        /// Provides invalid password user test cases for registration.
        /// </summary>
        /// <returns>An enumerable of invalid password user test cases.</returns>
        public static IEnumerable<UserRegisterDto> InvalidPasswordUserTestCases()
        {
            var list = AccountUserDataFactory.GetInvalidTestUserForRegisterList(20);
            foreach (var userRegister in list)
            {
                yield return userRegister;
            }
        }

        /// <summary>
        /// Test to verify that a bad response with user data is returned when trying to register with an existing email.
        /// </summary>
        [Test]
        public async Task Should_ReturnBadResponseWithUserData_When_RegisterWithExistentEmail()
        {
            _validUserRegistration = AccountUserDataFactory.CreateValidTestUserForRegister();

            Assert.IsNotNull(UserDto);

            // Act: Make the HTTP request to create an anonymous user
            var result = await HttpApiClient.MakeApiPostRequestAsync<UserDto>($"{AccountEndpoint.Register}", Is.EqualTo(HttpStatusCode.OK), _validUserRegistration);

            // Validate the API result data using a utility method
            Utilities.ValidateApiResult_ExpectedFailed(result);
            Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "email has been taken");
        }

        [Test]
        [TestCase("badformatted em,ail@ f")]
        [TestCase("badformatted@email@,c f")]
        [TestCase("badformatted@email@com.c o")]
        [TestCase("badformatted.email.com")]
        [TestCase("badformatted.email")]
        public async Task Should_ReturnBadResponseWithInvalidUserEmail_When_RegisterAccountUser(string email)
        {
            // Arrange: Prepare the user data for creation
            _validUserRegistration = AccountUserDataFactory.CreateValidTestUserForRegister();
            _validUserRegistration.Email = email;
            // Act: Make the HTTP request to create an anonymous user
            var result = await HttpApiClient.MakeApiPostRequestAsync<UserDto>($"{AccountEndpoint.Register}", Is.EqualTo(HttpStatusCode.OK).Or.EqualTo(HttpStatusCode.BadRequest), _validUserRegistration);

            // Validate the API result data using a utility method
            Utilities.ValidateApiResult_ExpectedFailed(result);

            // Assert that the User Id is not null or empty
            Utilities.ValidateApiResultMessage_ExpectContainsValue(result, new []{ "is not a valid", "is invalid" });
        }

        [Test(Description = "Test user account creation specifying bad password, expected to return BadRequest"), TestCaseSource(nameof(InvalidPasswordUserTestCases))]
        public async Task Should_ReturnBadRequestResponse_When_CreatingUserWithInvalidPassword(UserRegisterDto userRegister)
        {
            await HttpApiClient.MakeApiPostRequestAsync<object>($"{AccountEndpoint.Register}", Is.EqualTo(HttpStatusCode.BadRequest), userRegister);
        }

        [Test, TestCaseSource(nameof(ValidUserTestCases))]
        public async Task Should_ReturnOkResponseWithUserData_When_RegisterAccountUser(UserRegisterDto userRegister)
        {
            // Act: Make the HTTP request to create an anonymous user
            var result = await HttpApiClient.MakeApiPostRequestAsync<UserDto>($"{AccountEndpoint.Register}", Is.EqualTo(HttpStatusCode.OK), userRegister);
            
            // Validate the API result data using a utility method
            Utilities.ValidateApiResult_ExpectedSuccess(result);

            // Assert that the User Id is not null or empty
            Assert.IsTrue(!string.IsNullOrWhiteSpace(result.Data.Id), $"User Id must not be null/empty");
        }

        [Test()]
        public async Task Should_ReturnUnauthorizedResponseWithIncorrectPassword_When_UserRequestToken()
        {
            TokenRequest request = TokenDataFactory.CreateTokenRequestCustom(AccountUserDataFactory.CreateValidTestUserForRegister().Email,
                "NotValidPassword");

            var result = await HttpApiClient.MakeApiPostRequestAsync<TokenResponse>($"{AccountEndpoint.Login}", Is.EqualTo(HttpStatusCode.OK), request);

            Utilities.ValidateApiResult_ExpectedFailed(result);
            Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "invalid credentials");
        }

        [Test()]
        public async Task Should_ReturnOkResponseWithIncorrectEmail_When_UserRequestTokenWithUnregisteredEmail()
        {
            var user = AccountUserDataFactory.CreateValidTestUserForRegister();
            user.Email = "unregistered@email.com";
            TokenRequest request = TokenDataFactory.CreateTokenRequestCustom(user.Email, user.Password);

            var result = await HttpApiClient.MakeApiPostRequestAsync<TokenResponse>($"{AccountEndpoint.Login}", Is.EqualTo(HttpStatusCode.OK), request);

            Utilities.ValidateApiResult_ExpectedFailed(result);
            Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "not exist");
        }

        [Test()]
        public async Task Should_ReturnOkResponseWithTokenData_When_UserLoginAndCreateToken()
        {
            _validUserRegistration = AccountUserDataFactory.CreateValidTestUserForRegister();

            Assert.IsNotNull(UserDto);
            
            TokenRequest request =
                TokenDataFactory.CreateTokenRequestFromUserData(_validUserRegistration);

            var result = await HttpApiClient.MakeApiPostRequestAsync<TokenResponse>(
                $"{AccountEndpoint.Login}", Is.EqualTo(HttpStatusCode.OK), request);

            Utilities.ValidateApiResult_ExpectedSuccess(result);
            TokenResponse = result.Data;

            Assert.IsFalse(string.IsNullOrWhiteSpace(TokenResponse.Token), $"Token must not be null/empty");

            Assert.Less(DateTime.Now, TokenResponse.TokenExpiryTime, $"Token expiration time must be greater than actual date/time");
        }

        [Test()]
        public async Task Should_ReturnAuthorizedOkResponse_When_ValidatingUserToken()
        {
            Assert.IsNotNull(TokenResponse);

            HttpApiClient = CreateAuthorizedApiClient();

            var result = await HttpApiClient.MakeApiGetRequestAsync<UserDto>($"{AccountEndpoint.CurrentUser}", Is.EqualTo(HttpStatusCode.OK));

            Utilities.ValidateApiResult_ExpectedSuccess(result);
        }

        [Test()]
        public async Task Should_ReturnOkResponseWithUserDto_When_CallExistsEmail()
        {
            Assert.IsNotNull(TokenResponse);

            HttpApiClient = CreateAuthorizedApiClient();

            var result = await HttpApiClient.MakeApiGetRequestAsync<UserDto>($"{AccountEndpoint.ExistsEmail}?email={_validUserRegistration.Email}", Is.EqualTo(HttpStatusCode.OK));

            Utilities.ValidateApiResult_ExpectedSuccess(result);

            Assert.That(result.Data.Email, Is.EqualTo(_validUserRegistration.Email), "Email must be the equals to the supplied one to create user/token");
        }

        [Test()]
        public async Task Should_ReturnOkResponseWithValidEmail_When_CallExistsEmail()
        {
            Assert.IsNotNull(TokenResponse);
            string email = "notexistantEmail@d.com";

            HttpApiClient = CreateAuthorizedApiClient();

            var result = await HttpApiClient.MakeApiGetRequestAsync<UserDto>($"{AccountEndpoint.ExistsEmail}?email={email}", Is.EqualTo(HttpStatusCode.OK));

            Utilities.ValidateApiResult_ExpectedFailed(result);
            Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "not found");
        }

        /// <summary>
        /// Test to verify that an OK response with user data is returned when calling <see cref="AccountEndpoint.CurrentUser"/> with a valid token.
        /// </summary>
        [Test()]
        public async Task Should_ReturnOkResponseWithCurrentUser_When_ValidatingUserToken()
        {
            Assert.IsNotNull(TokenResponse);

            HttpApiClient = CreateAuthorizedApiClient();

            var result = await HttpApiClient.MakeApiGetRequestAsync<UserDto>($"{AccountEndpoint.CurrentUser}", Is.EqualTo(HttpStatusCode.OK));

            Utilities.ValidateApiResult_ExpectedSuccess(result);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result.Data.Id), $"must return user with valid id");

            Assert.That(result.Data.Email, Is.EqualTo(_validUserRegistration.Email), "Email must be the equals to the supplied one to create user/token");
        }
    }
}