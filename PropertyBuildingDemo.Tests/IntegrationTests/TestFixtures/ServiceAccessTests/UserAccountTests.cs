using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Entities.Identity;
using PropertyBuildingDemo.Tests.Factories;
using PropertyBuildingDemo.Tests.Helpers;
using System.Net;

namespace PropertyBuildingDemo.Tests.IntegrationTests.TestFixtures.ServiceAccessTests
{
    public class UserAccountTests : BaseTest
    {
        private UserRegisterDto validUserRegistration;

        [SetUp]
        public async Task Setup()
        {
            validUserRegistration = AccountUserDataFactory.CreateValidTestUserForRegister();
            await SetupUserDataAsync(validUserRegistration);
        }

        public static IEnumerable<UserRegisterDto> ValidUserTestCases()
        {
            var list = AccountUserDataFactory.GetValidTestUserForRegisterList(20);
            foreach (var userRegister in list)
            {
                yield return userRegister;
            }
        }
        public static IEnumerable<UserRegisterDto> InvalidPasswordUserTestCases()
        {
            var list = AccountUserDataFactory.GetInvalidTestUserForRegisterList(20);
            foreach (var userRegister in list)
            {
                yield return userRegister;
            }
        }

        [Test]
        public async Task Should_ReturnBadResponseWithUserData_When_RegisterWithExistentEmail()
        {
            validUserRegistration = AccountUserDataFactory.CreateValidTestUserForRegister();

            Assert.IsNotNull(_userDto);

            // Act: Make the HTTP request to create an anonymous user
            var result = await httpApiClient.MakeApiPostRequestAsync<UserDto>($"{TestConstants.AccountEnpoint.Register}", Is.EqualTo(HttpStatusCode.OK), validUserRegistration);

            // Validate the API result data using a utility method
            Utilities.ValidateApiResult_ExpectedNotOk(result);
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
            validUserRegistration = AccountUserDataFactory.CreateValidTestUserForRegister();
            validUserRegistration.Email = email;
            // Act: Make the HTTP request to create an anonymous user
            var result = await httpApiClient.MakeApiPostRequestAsync<UserDto>($"{TestConstants.AccountEnpoint.Register}", Is.EqualTo(HttpStatusCode.OK).Or.EqualTo(HttpStatusCode.BadRequest), validUserRegistration);

            // Validate the API result data using a utility method
            Utilities.ValidateApiResult_ExpectedNotOk(result);

            // Assert that the User Id is not null or empty
            Utilities.ValidateApiResultMessage_ExpectContainsValue(result, new []{ "is not a valid", "is invalid" });
        }

        [Test(Description = "Test user account creation specifying bad password, expected to return BadRequest"), TestCaseSource(nameof(InvalidPasswordUserTestCases))]
        public async Task Should_ReturnBadRequestResponse_When_CreatingUserWithInvalidPassword(UserRegisterDto userRegister)
        {
            await httpApiClient.MakeApiPostRequestAsync<object>($"{TestConstants.AccountEnpoint.Register}", Is.EqualTo(HttpStatusCode.BadRequest), userRegister);
        }

        [Test, TestCaseSource(nameof(ValidUserTestCases))]
        public async Task Should_ReturnOkResponseWithUserData_When_RegisterAccountUser(UserRegisterDto userRegister)
        {
            // Act: Make the HTTP request to create an anonymous user
            var result = await httpApiClient.MakeApiPostRequestAsync<UserDto>($"{TestConstants.AccountEnpoint.Register}", Is.EqualTo(HttpStatusCode.OK), userRegister);
            
            // Validate the API result data using a utility method
            Utilities.ValidateApiResultData(result);

            // Assert that the User Id is not null or empty
            Assert.IsTrue(!string.IsNullOrWhiteSpace(result.Data.Id), $"User Id must not be null/empty");
        }

        [Test()]
        public async Task Should_ReturnUnauthorizedResponseWithIncorrectPassword_When_UserRequestToken()
        {
            TokenRequest request = TokenDataFactory.CreateTokenRequestCustom(AccountUserDataFactory.CreateValidTestUserForRegister().Email,
                "NotValidPassword");

            var result = await httpApiClient.MakeApiPostRequestAsync<TokenResponse>($"{TestConstants.AccountEnpoint.Login}", Is.EqualTo(HttpStatusCode.OK), request);

            Utilities.ValidateApiResult_ExpectedNotOk(result);
            Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "invalid credentials");
        }

        [Test()]
        public async Task Should_ReturnOkResponseWithIncorrecEmail_When_UserRequestTokenWithUnregisteredEmail()
        {
            var user = AccountUserDataFactory.CreateValidTestUserForRegister();
            user.Email = "unregistered@email.com";
            TokenRequest request = TokenDataFactory.CreateTokenRequestCustom(user.Email, user.Password);

            var result = await httpApiClient.MakeApiPostRequestAsync<TokenResponse>($"{TestConstants.AccountEnpoint.Login}", Is.EqualTo(HttpStatusCode.OK), request);

            Utilities.ValidateApiResult_ExpectedNotOk(result);
            Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "not exist");
        }

        [Test()]
        public async Task Should_ReturnOkResponseWithTokenData_When_UserLoginAndCreateToken()
        {
            validUserRegistration = AccountUserDataFactory.CreateValidTestUserForRegister();

            Assert.IsNotNull(_userDto);
            
            TokenRequest request =
                TokenDataFactory.CreateTokenRequestFromUserData(validUserRegistration);

            var result = await httpApiClient.MakeApiPostRequestAsync<TokenResponse>(
                $"{TestConstants.AccountEnpoint.Login}", Is.EqualTo(HttpStatusCode.OK), request);

            Utilities.ValidateApiResultData(result);
            _tokenResponse = result.Data;

            Assert.IsFalse(string.IsNullOrWhiteSpace(_tokenResponse.Token), $"Token must not be null/empty");

            Assert.Less(DateTime.Now, _tokenResponse.TokenExpiryTime, $"Token expiration time must be greater than actual date/time");
        }

        [Test()]
        public async Task Should_ReturnAuthorizedOkResponse_When_ValidatingUserToken()
        {
            Assert.IsNotNull(_tokenResponse);

            httpApiClient = CreateAuthorizedApiClient();

            var result = await httpApiClient.MakeApiGetRequestAsync<UserDto>($"{TestConstants.AccountEnpoint.CurrentUser}", Is.EqualTo(HttpStatusCode.OK));

            Utilities.ValidateApiResultData(result);
        }

        [Test()]
        public async Task Should_ReturnOkResponseWithUserDto_When_CallExistsEmail()
        {
            Assert.IsNotNull(_tokenResponse);

            httpApiClient = CreateAuthorizedApiClient();

            var result = await httpApiClient.MakeApiGetRequestAsync<UserDto>($"{TestConstants.AccountEnpoint.ExistsEmail}?email={validUserRegistration.Email}", Is.EqualTo(HttpStatusCode.OK));

            Utilities.ValidateApiResultData(result);

            Assert.That(result.Data.Email, Is.EqualTo(validUserRegistration.Email), "Email must be the equals to the supplied one to create user/token");
        }

        [Test()]
        public async Task Should_ReturnOkResponseWithValidEmail_When_CallExistsEmail()
        {
            Assert.IsNotNull(_tokenResponse);
            string email = "notexistantEmail@d.com";

            httpApiClient = CreateAuthorizedApiClient();

            var result = await httpApiClient.MakeApiGetRequestAsync<UserDto>($"{TestConstants.AccountEnpoint.ExistsEmail}?email={email}", Is.EqualTo(HttpStatusCode.OK));

            Utilities.ValidateApiResult_ExpectedNotOk(result);
            Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "not found");
        }

        [Test()]
        public async Task Should_ReturnOkResponseWithCurrentUser_When_ValidatingUserToken()
        {
            Assert.IsNotNull(_tokenResponse);

            httpApiClient = CreateAuthorizedApiClient();

            var result = await httpApiClient.MakeApiGetRequestAsync<UserDto>($"{TestConstants.AccountEnpoint.CurrentUser}", Is.EqualTo(HttpStatusCode.OK));

            Utilities.ValidateApiResultData(result);

            Assert.IsFalse(string.IsNullOrWhiteSpace(result.Data.Id), $"must return user with valid id");

            Assert.That(result.Data.Email, Is.EqualTo(validUserRegistration.Email), "Email must be the equals to the supplied one to create user/token");
        }
    }
}