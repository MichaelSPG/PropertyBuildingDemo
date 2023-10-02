using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Tests.Helpers;
using System.Net;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Tests.Factories;

namespace PropertyBuildingDemo.Tests.IntegrationTests.TestFixtures;

[TestFixture]
public class PropertyBuildingIntegrationTests : BaseTest
{
    protected IDataFactory<OwnerDto> OwnerDataFactory;
    protected IDataFactory<PropertyDto> PropertyDataFactory;

    private List<OwnerDto> _ownerValidList;

    private List<PropertyDto> _propertyValidList;

    protected const int ValidTestEntityCount = 10;

    [SetUp]
    public async Task Setup()
    {
        ValidUserRegistration = AccountUserDataFactory.CreateValidTestUserForRegister();
        await SetupUserDataAsync(ValidUserRegistration);
        HttpApiClient = CreateAuthorizedApiClient();

        OwnerDataFactory = EntityDataFactory.GetFactory<OwnerDto>();
        PropertyDataFactory = EntityDataFactory.GetFactory<PropertyDto>();

        _propertyValidList = PropertyDataFactory.CreateValidEntityDtoList(ValidTestEntityCount).ToList();

        _ownerValidList = OwnerDataFactory.CreateValidEntityDtoList(ValidTestEntityCount).ToList();

        _ownerValidList = await InsertListOfEntity<Owner, OwnerDto>(_ownerValidList);
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithPropertyNotFoundMessage_When_PropertyHasInvalidId()
    {
        long notExistentId = 945893;
        var result = await HttpApiClient.MakeApiGetRequestAsync<PropertyDto>($"{TestConstants.PropertyBuildingEnpoint.ById}?id={notExistentId}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedFailed(result);
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "not exist");
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithPropertyData_When_InsertPropertyWithValidDataWithNoImagesOrTraces()
    {
        Assert.That(_ownerValidList, Is.Not.Null);
        Assert.That(_ownerValidList, Is.Not.Empty);

        var ownerId = _ownerValidList[Utilities.Random.Next(0, _ownerValidList.Count)].IdOwner;

        Assert.That(ownerId, Is.GreaterThan(0));

        var property = PropertyDataFactory.CreateValidEntityDto();

        property.IdOwner = ownerId;

        var result = await HttpApiClient.MakeApiPostRequestAsync<PropertyDto>($"{TestConstants.PropertyBuildingEnpoint.Insert}", 
            Is.EqualTo(HttpStatusCode.OK), property);

        Utilities.ValidateApiResultData_ExpectedSuccess(result);
        Assert.That(result.Data.IdProperty, Is.GreaterThan(0));
    }


}