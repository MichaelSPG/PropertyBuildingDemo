using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Tests.Helpers;
using System.Net;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Domain.Entities.Enums;
using PropertyBuildingDemo.Tests.Factories;

namespace PropertyBuildingDemo.Tests.IntegrationTests.TestFixtures;

[TestFixture]
public class PropertyBuildingIntegrationTests : BaseTest
{
    private List<OwnerDto> _ownerValidList;
    private List<PropertyDto> _propertyValidList;
    protected const int ValidTestEntityCount = 10;

    List<long> GetValidOwnerIdList(List<OwnerDto> owners = null)
    {
        if (owners == null)
        {
            owners = _ownerValidList;
        }
        return owners.Select(x => x.IdOwner).ToList();
    }

    async Task<List<PropertyDto>> InsertValidPropertyDtoList(int count, long ownerId = 0, bool sameOwner = true, int minImages = 0, int minTraces = 0)
    {
        return await this.InsertListOfEntity<Property, PropertyDto>(
            PropertyBuildingDataFactory.GenerateRandomValidProperties(
                count, 
                GetValidOwnerIdList(),
                ownerId, 
                sameOwner,
                minImages, minTraces));
    }

    [SetUp]
    public async Task Setup()
    {
        await SetupValidRegistrationUser();

        _propertyValidList = PropertyBuildingDataFactory.PropertyDataFactory.CreateValidEntityDtoList(ValidTestEntityCount).ToList();
        _ownerValidList = PropertyBuildingDataFactory.OwnerDataFactory.CreateValidEntityDtoList(ValidTestEntityCount).ToList();

        _ownerValidList = await InsertListOfEntity<Owner, OwnerDto>(_ownerValidList);
    }

    #region QUERY

    [Test()]
    public async Task Should_ReturnOkResponseWithPropertyNotFoundMessage_When_GetPropertyWithInvalidId()
    {
        long notExistentId = 945893;
        var result = await HttpApiClient.MakeApiGetRequestAsync<PropertyDto>($"{TestConstants.PropertyBuildingEnpoint.ById}/{notExistentId}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedFailed(result);
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "not exist");
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithPropertyData_When_FilterPropertyIdEqualsOneResult()
    {
        var currentProperty = await InsertValidPropertyDtoList(1, 0, false, 4, 2);
        var targetEntity = currentProperty.FirstOrDefault();


        DefaultQueryFilterArgs args = new DefaultQueryFilterArgs();
        args.FilteringParameters = new List<FilteringParameters>()
        {
            new("IdProperty", targetEntity.IdProperty.ToString(), EComparisionOperator.Equal),
        };
        args.PageIndex = 0;
        args.PageSize = 10;

        var result = await HttpApiClient.MakeApiPostRequestAsync<List<PropertyDto>>(
            $"{TestConstants.PropertyBuildingEnpoint.ListBy}",
            Is.EqualTo(HttpStatusCode.OK), args);

        Utilities.ValidateApiResultData_ExpectedSuccess(result);

        Assert.That(result.Data.Count, Is.EqualTo(1));
        Assert.That(result.Data.FirstOrDefault().IdProperty, Is.EqualTo(targetEntity.IdProperty));
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithPropertyData_When_FilterPropertyIdNotEqualsOneResult()
    {
        int countTotal = 20;
        var currentProperty = await this.InsertValidPropertyDtoList(countTotal,
                0, false, 4, 2);
        var targetEntity = currentProperty.FirstOrDefault();


        DefaultQueryFilterArgs args = new DefaultQueryFilterArgs();
        args.FilteringParameters = new List<FilteringParameters>()
        {
            new("Owner.IdOwner", targetEntity.IdOwner.ToString(), EComparisionOperator.NotEqual),
        };

        var result = await HttpApiClient.MakeApiPostRequestAsync<List<PropertyDto>>(
            $"{TestConstants.PropertyBuildingEnpoint.ListBy}",
            Is.EqualTo(HttpStatusCode.OK), args);

        Utilities.ValidateApiResultData_ExpectedSuccess(result);
        
        Assert.That(result.Data.Count, Is.LessThan(countTotal));
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithPagingPropertyData_When_FilterPropertyNoFilters()
    {
        var currentProperty = await this.InsertValidPropertyDtoList(  20, 0, false, 4, 2);
        var targetEntity = currentProperty.FirstOrDefault();

        DefaultQueryFilterArgs args = new DefaultQueryFilterArgs();
        args.PageIndex = 0;
        args.PageSize = 10;

        var result = await HttpApiClient.MakeApiPostRequestAsync<List<PropertyDto>>(
            $"{TestConstants.PropertyBuildingEnpoint.ListBy}",
            Is.EqualTo(HttpStatusCode.OK), args);

        Utilities.ValidateApiResultData_ExpectedSuccess(result);

        Assert.That(result.Data.Count, Is.EqualTo(args.PageSize));
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithPropertyData_When_FilterPropertyIdContainsOneResult()
    {
        string criteria = "Christoper";
        var targetEntity = PropertyBuildingDataFactory.PropertyDataFactory.CreateValidEntityDto();
        targetEntity.IdOwner = _ownerValidList.FirstOrDefault().IdOwner;

        targetEntity = await this.InsertValidEntityDto< Property, PropertyDto> (targetEntity);
        
        var currentProperty = await this.InsertValidPropertyDtoList(1,  0, false, 4, 2);
        
        DefaultQueryFilterArgs args = new DefaultQueryFilterArgs();
        args.FilteringParameters = new List<FilteringParameters>()
        {
            new("Name", criteria, EComparisionOperator.StartsWith),
        };
        args.PageIndex = 0;
        args.PageSize = 10;

        var result = await HttpApiClient.MakeApiPostRequestAsync<List<PropertyDto>>(
            $"{TestConstants.PropertyBuildingEnpoint.ListBy}",
            Is.EqualTo(HttpStatusCode.OK), args);

        Utilities.ValidateApiResultData_ExpectedSuccess(result);

        Assert.That(result.Data.Count, Is.EqualTo(1));
        Assert.That(result.Data.FirstOrDefault().IdProperty, Is.EqualTo(targetEntity.IdProperty));
    }
    #endregion

    #region INSERT_TEST

    [Test()]
    [TestCase(true, 0, 0)]
    [TestCase(true, 10, 0)]
    [TestCase(true, 0, 10)]
    [TestCase(true, 10, 10)]
    public async Task Should_ReturnOkResponseWithSinglePropertyData_When_InsertSinglePropertyWithValidData(bool sameOwner, int numImages, int numTraces)
    {
        var validOwners = await GetEntityListWithApi<OwnerDto>(TestConstants.OwnerEndpoint.BaseEndpoint + "/List");

        var property = PropertyBuildingDataFactory.GenerateRandomValidProperties(1, GetValidOwnerIdList(validOwners), 0, sameOwner, numImages, numTraces).FirstOrDefault();
        
        var result = await HttpApiClient.MakeApiPostRequestAsync<PropertyDto>($"{TestConstants.PropertyBuildingEnpoint.Insert}",
            Is.EqualTo(HttpStatusCode.OK), property);

        Utilities.ValidateApiResultData_ExpectedSuccess(result);

        var insertedEntity = await GetEntityDto<Property, PropertyDto>(result.Data.IdProperty);
        

        Assert.That(result.Data.IdProperty, Is.GreaterThan(0));
        Assert.That(insertedEntity, Is.Not.Null);
        Assert.That(insertedEntity.PropertyImages.Count(), Is.EqualTo(numImages));
        Assert.That(insertedEntity.PropertyTraces.Count(), Is.EqualTo(numTraces));
    }

    [Test()]
    [TestCase(true, 1)]
    [TestCase(true, 4)]
    [TestCase(true, 5)]
    [TestCase(true, 10)]
    public async Task Should_ReturnOkResponseWithSinglePropertyData_When_InsertImageWithValidData(bool sameOwner, int initialImages)
    {
        var currentProperty = await this.InsertValidEntityDto<Property, PropertyDto>(PropertyBuildingDataFactory.GenerateRandomValidProperties(1, GetValidOwnerIdList(), 0, sameOwner, initialImages).FirstOrDefault());

        var newImages = PropertyBuildingDataFactory.PropertyImageDataFactory.CreateValidEntityDtoList(1, currentProperty.IdProperty).FirstOrDefault();

        var result = await HttpApiClient.MakeApiPostRequestAsync<PropertyDto>($"{TestConstants.PropertyBuildingEnpoint.AddImage}",
            Is.EqualTo(HttpStatusCode.OK), newImages);

        var insertedEntity = await GetEntityWithApi<PropertyDto>($"{TestConstants.PropertyBuildingEnpoint.ById}/{result.Data.IdProperty}");
        //await GetEntityDto<Property, PropertyDto>(result.Data.IdProperty);
        Utilities.ValidateApiResultData_ExpectedSuccess(result);

        Assert.That(result.Data.IdProperty, Is.GreaterThan(0));
        Assert.That(insertedEntity, Is.Not.Null);
        Assert.That(insertedEntity.PropertyImages.Count(), Is.EqualTo(initialImages + 1));
    }



    #endregion


    #region UPDATE_TESTS

    [Test()]
    [TestCase(true, 0, 0, 10, 0)]
    [TestCase(true, 10, 0, 0, 10)]
    [TestCase(true, 0, 10, 0, 23)]
    [TestCase(true, 10, 10, 20, 4)]
    public async Task Should_ReturnOkResponseWithPropertyData_When_UpdatePropertyWithValidDataNoImagesOrTraces(bool sameOwner, int initialImages, int initialTraces, int numImages, int numTraces)
    {
        var currentProperty = await this.InsertValidEntityDto<Property, PropertyDto>(PropertyBuildingDataFactory.GenerateRandomValidProperties(1, GetValidOwnerIdList(), 0, sameOwner, initialImages, initialTraces).FirstOrDefault());

        var expectedProperty = PropertyBuildingDataFactory.GenerateRandomValidProperties(1, GetValidOwnerIdList(), currentProperty.IdOwner, sameOwner, numImages, numTraces).FirstOrDefault();
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
        var currentProperty = await this.InsertValidEntityDto<Property, PropertyDto>(PropertyBuildingDataFactory.GenerateRandomValidProperties(1, GetValidOwnerIdList(), 0, false, 4, 2).FirstOrDefault());

        var result = await HttpApiClient.MakeApiPutRequestAsync<PropertyDto>(
            $"{TestConstants.PropertyBuildingEnpoint.ChangePrice}/{currentProperty.IdProperty}?inNewPrice={newPrice}",
            Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResultData_ExpectedSuccess(result);

        var updatedEntity = await GetEntityWithApi<PropertyDto>($"{TestConstants.PropertyBuildingEnpoint.ById}/{result.Data.IdProperty}");

        Assert.That(result.Data.IdProperty, Is.EqualTo(updatedEntity.IdProperty));
        Assert.That(updatedEntity.Price, Is.EqualTo(newPrice));
    }
    #endregion

}