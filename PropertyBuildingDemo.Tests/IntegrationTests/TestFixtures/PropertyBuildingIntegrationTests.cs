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
    protected IDataFactory<PropertyTraceDto> PropertyTraceDataFactory;
    protected IDataFactory<PropertyImageDto> PropertyImageDataFactory;
    private List<OwnerDto> _ownerValidList;

    private List<PropertyDto> _propertyValidList;

    protected const int ValidTestEntityCount = 10;

    protected List<PropertyDto> GenerateRandomValidProperties(int count, long ownerId = 0, bool sameOwner = false, int minImages = 0, int minTraces = 0)
    {
        var propertyList = PropertyDataFactory.CreateValidEntityDtoList(count);
        if (ownerId <= 0)
        {
            ownerId = _ownerValidList[Utilities.Random.Next(0, _ownerValidList.Count)].IdOwner;
        }

        foreach (var property in propertyList)
        {
            if (minImages > 0)
            {
                property.PropertyImages = PropertyImageDataFactory.CreateValidEntityDtoList(minImages);
            }

            if (minTraces > 0)
            {
                property.PropertyTraces = PropertyTraceDataFactory.CreateValidEntityDtoList(minTraces);
            }

            if (!sameOwner)
            {
                ownerId = _ownerValidList[Utilities.Random.Next(0, _ownerValidList.Count)].IdOwner;
            }
            property.IdOwner = ownerId;
        }
        return propertyList.ToList();
    }

    [SetUp]
    public async Task Setup()
    {
        ValidUserRegistration = AccountUserDataFactory.CreateValidTestUserForRegister();
        await SetupUserDataAsync(ValidUserRegistration);
        HttpApiClient = CreateAuthorizedApiClient();

        OwnerDataFactory = EntityDataFactory.GetFactory<OwnerDto>();
        PropertyDataFactory = EntityDataFactory.GetFactory<PropertyDto>();
        PropertyTraceDataFactory = EntityDataFactory.GetFactory<PropertyTraceDto>();
        PropertyImageDataFactory = EntityDataFactory.GetFactory<PropertyImageDto>();

        _propertyValidList = PropertyDataFactory.CreateValidEntityDtoList(ValidTestEntityCount).ToList();

        _ownerValidList = OwnerDataFactory.CreateValidEntityDtoList(ValidTestEntityCount).ToList();

        _ownerValidList = await InsertListOfEntity<Owner, OwnerDto>(_ownerValidList);
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithPropertyNotFoundMessage_When_GetPropertyWithInvalidId()
    {
        long notExistentId = 945893;
        var result = await HttpApiClient.MakeApiGetRequestAsync<PropertyDto>($"{TestConstants.PropertyBuildingEnpoint.ById}/{notExistentId}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedFailed(result);
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "not exist");
    }
    
    [Test()]
    [TestCase( true, 0, 0)]
    [TestCase( true, 10, 0)]
    [TestCase( true, 0, 10)]
    [TestCase( true, 10, 10)]
    public async Task Should_ReturnOkResponseWithSinglePropertyData_When_InsertSinglePropertyWithValidData(bool sameOwner, int numImages, int numTraces)
    {
        var property = GenerateRandomValidProperties(1, 0, sameOwner, numImages, numTraces).FirstOrDefault();

        var result = await HttpApiClient.MakeApiPostRequestAsync<PropertyDto>($"{TestConstants.PropertyBuildingEnpoint.Insert}",
            Is.EqualTo(HttpStatusCode.OK), property);

        var insertedEntity = await GetEntityDto<Property, PropertyDto>(result.Data.IdProperty);
        Utilities.ValidateApiResultData_ExpectedSuccess(result);

        Assert.That(result.Data.IdProperty, Is.GreaterThan(0));
        Assert.That(insertedEntity, Is.Not.Null);
        Assert.That(insertedEntity.PropertyImages.Count(), Is.EqualTo(numImages));
        Assert.That(insertedEntity.PropertyTraces.Count(), Is.EqualTo(numTraces));
    }

    [Test()]
    [TestCase(true, 0, 0, 10, 0)]
    [TestCase(true, 10, 0, 0, 10)]
    [TestCase(true, 0, 10, 0, 23)]
    [TestCase(true, 10, 10, 20, 4)]
    public async Task Should_ReturnOkResponseWithPropertyData_When_UpdatePropertyWithValidDataNoImagesOrTraces(bool sameOwner, int initialImages, int initialTraces, int numImages, int numTraces)
    {
        var currentProperty = await this.InsertValidEntityDto<Property, PropertyDto>(GenerateRandomValidProperties(1, 0, sameOwner, initialImages, initialTraces).FirstOrDefault());
        
        var expectedProperty = GenerateRandomValidProperties(1, currentProperty.IdOwner, sameOwner, numImages, numTraces).FirstOrDefault();
        expectedProperty.IdProperty = currentProperty.IdProperty;

        var result = await HttpApiClient.MakeApiPutRequestAsync<PropertyDto>($"{TestConstants.PropertyBuildingEnpoint.Update}",
            Is.EqualTo(HttpStatusCode.OK), expectedProperty);

        Utilities.ValidateApiResultData_ExpectedSuccess(result);

        var updatedEntity = await GetEntityWithApi<PropertyDto>($"{TestConstants.PropertyBuildingEnpoint.ById}/{result.Data.IdProperty}");
        Assert.That(result.Data.IdProperty, Is.EqualTo(expectedProperty.IdProperty));

        Assert.That(expectedProperty.PropertyImages.Count(), Is.EqualTo(numImages));
        Assert.That(expectedProperty.PropertyTraces.Count(), Is.EqualTo(numTraces));
        Assert.That(expectedProperty.IdOwner, Is.EqualTo(updatedEntity.IdOwner));
        Assert.That(expectedProperty.Price, Is.EqualTo(updatedEntity.Price));
        Assert.That(expectedProperty.Name, Is.EqualTo(updatedEntity.Name));
        Assert.That(expectedProperty.Address, Is.EqualTo(updatedEntity.Address));
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithPropertyData_When_UpdatePropertyPriceWithValidId()
    {
        decimal newPrice = 1419232.22M;
        var currentProperty = await this.InsertValidEntityDto<Property, PropertyDto>(GenerateRandomValidProperties(1, 0, false, 4, 2).FirstOrDefault());
        
        var result = await HttpApiClient.MakeApiPutRequestAsync<PropertyDto>(
            $"{TestConstants.PropertyBuildingEnpoint.ChangePrice}/{currentProperty.IdProperty}?inNewPrice={newPrice}",
            Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResultData_ExpectedSuccess(result);

        var updatedEntity = await GetEntityWithApi<PropertyDto>($"{TestConstants.PropertyBuildingEnpoint.ById}/{result.Data.IdProperty}");

        Assert.That(result.Data.IdProperty, Is.EqualTo(updatedEntity.IdProperty));
        Assert.That(updatedEntity.Price, Is.EqualTo(newPrice));
    }

    [Test()]
    [TestCase(true, 1 )]
    [TestCase(true, 4)]
    [TestCase(true, 5)]
    [TestCase(true, 10)]
    public async Task Should_ReturnOkResponseWithSinglePropertyData_When_InsertImageWithValidData(bool sameOwner, int initialImages)
    {
        var currentProperty = await this.InsertValidEntityDto<Property, PropertyDto>(GenerateRandomValidProperties(1, 0, sameOwner, initialImages).FirstOrDefault());

        var newImages = PropertyImageDataFactory.CreateValidEntityDtoList(1, currentProperty.IdProperty).FirstOrDefault();

        var result = await HttpApiClient.MakeApiPostRequestAsync<PropertyDto>($"{TestConstants.PropertyBuildingEnpoint.AddImage}",
            Is.EqualTo(HttpStatusCode.OK), newImages);

        var insertedEntity = await GetEntityWithApi<PropertyDto>($"{TestConstants.PropertyBuildingEnpoint.ById}/{result.Data.IdProperty}");
        //await GetEntityDto<Property, PropertyDto>(result.Data.IdProperty);
        Utilities.ValidateApiResultData_ExpectedSuccess(result);

        Assert.That(result.Data.IdProperty, Is.GreaterThan(0));
        Assert.That(insertedEntity, Is.Not.Null);
        Assert.That(insertedEntity.PropertyImages.Count(), Is.EqualTo(initialImages+1));
    }
}