using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Tests.Factories;
using PropertyBuildingDemo.Tests.Helpers;
using System.Net;
using PropertyBuildingDemo.Domain.Entities.Identity;

namespace PropertyBuildingDemo.Tests.IntegrationTests.TestFixtures.PropertyBuildingTests;

[TestFixture]
public class CreationTests : BaseTest
{
    private UserRegisterDto validUserRegistration;

    [SetUp]
    public async Task Setup()
    {
        validUserRegistration = AccountUserDataFactory.CreateValidTestUserForRegister();
        await SetupUserDataAsync(validUserRegistration);
        httpApiClient = CreateAuthorizedApiClient();
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithPropertyNotFoundMessage_When_PropertyHasInvalidId()
    {
        long notExistentId = 945893;
        var result = await httpApiClient.MakeApiGetRequestAsync<PropertyDto>($"{TestConstants.PropertyBuildingEnpoint.ById}?id={notExistentId}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedNotOk(result);
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "not exist");
    }
}