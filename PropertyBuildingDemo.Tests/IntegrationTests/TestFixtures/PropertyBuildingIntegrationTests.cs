using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Tests.Helpers;
using System.Net;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Domain.Entities.Enums;
using PropertyBuildingDemo.Tests.Factories;
using PropertyBuildingDemo.Tests.Helpers.Config;

namespace PropertyBuildingDemo.Tests.IntegrationTests.TestFixtures;

/// <summary>
/// Represents a test fixture for Property Building integration tests.
/// </summary>
[TestFixture]
public class PropertyBuildingIntegrationTests : PropertyBaseTest
{
    [SetUp]
    public async Task Setup()
    {
        await SetupValidRegistrationUser();
        await SetupPropertyTest();
    }

    #region QUERY

    /// <summary>
    /// Test to verify that an OK response with a "Property not found" message is returned when getting a property with an invalid ID.
    /// </summary>
    [Test()]
    public async Task Should_ReturnOkResponseWithPropertyNotFoundMessage_When_GetPropertyWithInvalidId()
    {
        long notExistentId = 945893;
        var result = await HttpApiClient.MakeApiGetRequestAsync<PropertyDto>($"{PropertyBuildingEnpoint.ById}/{notExistentId}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedFailed(result);
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "not exist");
    }

    /// <summary>
    /// Test to verify that an OK response with property data is returned when filtering by property ID, resulting in one property.
    /// </summary>
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
            $"{PropertyBuildingEnpoint.ListBy}",
            Is.EqualTo(HttpStatusCode.OK), args);

        Utilities.ValidateApiResult_ExpectedSuccess(result);

        Assert.That(result.Data.Count, Is.EqualTo(1));
        Assert.That(result.Data.FirstOrDefault().IdProperty, Is.EqualTo(targetEntity.IdProperty));
    }

    /// <summary>
    /// Test to verify that an OK response with property data is returned when filtering by property ID, resulting in multiple properties.
    /// </summary>
    [Test()]
    public async Task Should_ReturnOkResponseWithPropertyData_When_FilterPropertyIdNotEqualsOneResult()
    {
        int countTotal = 20;
        var currentProperty = await InsertValidPropertyDtoList(countTotal, 0, false, 4, 2);
        var targetEntity = currentProperty.FirstOrDefault();

        DefaultQueryFilterArgs args = new DefaultQueryFilterArgs();
        args.FilteringParameters = new List<FilteringParameters>()
    {
        new("Owner.IdOwner", targetEntity.IdOwner.ToString(), EComparisionOperator.NotEqual),
    };

        var result = await HttpApiClient.MakeApiPostRequestAsync<List<PropertyDto>>(
            $"{PropertyBuildingEnpoint.ListBy}",
            Is.EqualTo(HttpStatusCode.OK), args);

        Utilities.ValidateApiResult_ExpectedSuccess(result);

        Assert.That(result.Data.Count, Is.LessThan(countTotal));
    }

    /// <summary>
    /// Test to verify that an OK response with paging property data is returned when no filters are applied.
    /// </summary>
    [Test()]
    public async Task Should_ReturnOkResponseWithPagingPropertyData_When_FilterPropertyNoFilters()
    {
        var currentProperty = await InsertValidPropertyDtoList(20, 0, false, 4, 2);
        var targetEntity = currentProperty.FirstOrDefault();

        DefaultQueryFilterArgs args = new DefaultQueryFilterArgs();
        args.PageIndex = 0;
        args.PageSize = 10;

        var result = await HttpApiClient.MakeApiPostRequestAsync<List<PropertyDto>>(
            $"{PropertyBuildingEnpoint.ListBy}",
            Is.EqualTo(HttpStatusCode.OK), args);

        Utilities.ValidateApiResult_ExpectedSuccess(result);

        Assert.That(result.Data.Count, Is.EqualTo(args.PageSize));
    }

    /// <summary>
    /// Test to verify that an OK response with property data is returned when filtering by property name, resulting in one property.
    /// </summary>
    [Test()]
    public async Task Should_ReturnOkResponseWithPropertyData_When_FilterPropertyIdContainsOneResult()
    {
        string criteria = "Christoper";
        var targetEntity = PropertyBuildingDataFactory.PropertyDataFactory.CreateValidEntityDto();
        targetEntity.IdOwner = OwnerValidList.FirstOrDefault().IdOwner;

        targetEntity = await this.InsertValidEntityDto<Property, PropertyDto>(targetEntity);

        var currentProperty = await this.InsertValidPropertyDtoList(1, 0, false, 4, 2);

        DefaultQueryFilterArgs args = new DefaultQueryFilterArgs();
        args.FilteringParameters = new List<FilteringParameters>()
    {
        new("Name", criteria, EComparisionOperator.StartsWith),
    };
        args.PageIndex = 0;
        args.PageSize = 10;

        var result = await HttpApiClient.MakeApiPostRequestAsync<List<PropertyDto>>(
            $"{PropertyBuildingEnpoint.ListBy}",
            Is.EqualTo(HttpStatusCode.OK), args);

        Utilities.ValidateApiResult_ExpectedSuccess(result);

        Assert.That(result.Data.Count, Is.EqualTo(1));
        Assert.That(result.Data.FirstOrDefault().IdProperty, Is.EqualTo(targetEntity.IdProperty));
    }
    #endregion

    #region INSERT_TEST

    /// <summary>
    /// Test to verify that an OK response with single property data is returned when inserting a property with valid data.
    /// </summary>
    /// <param name="sameOwner">Indicates whether the property has the same owner as an existing property.</param>
    /// <param name="numImages">The number of images to associate with the property.</param>
    /// <param name="numTraces">The number of traces to associate with the property.</param>
    [Test()]
    [TestCase(true, 0, 0)]
    [TestCase(true, 10, 0)]
    [TestCase(true, 0, 10)]
    [TestCase(true, 10, 10)]
    public async Task Should_ReturnOkResponseWithSinglePropertyData_When_InsertSinglePropertyWithValidData(bool sameOwner, int numImages, int numTraces)
    {
        var validOwners = await GetEntityListWithApi<OwnerDto>(OwnerEndpoint.BaseEndpoint + "/List");

        var property = PropertyBuildingDataFactory.GenerateRandomValidProperties(1, GetValidOwnerIdList(validOwners), 0, sameOwner, numImages, numTraces).FirstOrDefault();

        var result = await HttpApiClient.MakeApiPostRequestAsync<PropertyDto>($"{PropertyBuildingEnpoint.Insert}",
            Is.EqualTo(HttpStatusCode.OK), property);

        Utilities.ValidateApiResult_ExpectedSuccess(result);

        var insertedEntity = await GetEntityDto<Property, PropertyDto>(result.Data.IdProperty);

        Assert.That(result.Data.IdProperty, Is.GreaterThan(0));
        Assert.That(insertedEntity, Is.Not.Null);
        Assert.That(insertedEntity.PropertyImages.Count(), Is.EqualTo(numImages));
        Assert.That(insertedEntity.PropertyTraces.Count(), Is.EqualTo(numTraces));
    }

    /// <summary>
    /// Test to verify that an OK response with single property data is returned when inserting an image with valid data.
    /// </summary>
    /// <param name="sameOwner">Indicates whether the property has the same owner as an existing property.</param>
    /// <param name="initialImages">The initial number of images associated with the property.</param>
    [Test()]
    [TestCase(true, 1)]
    [TestCase(true, 4)]
    [TestCase(true, 5)]
    [TestCase(true, 10)]
    public async Task Should_ReturnOkResponseWithSinglePropertyData_When_InsertImageWithValidData(bool sameOwner, int initialImages)
    {
        var currentProperty = await this.InsertValidEntityDto<Property, PropertyDto>(PropertyBuildingDataFactory.GenerateRandomValidProperties(1, GetValidOwnerIdList(), 0, sameOwner, initialImages).FirstOrDefault());

        var newImages = PropertyBuildingDataFactory.PropertyImageDataFactory.CreateValidEntityDtoList(1, currentProperty.IdProperty).FirstOrDefault();

        var result = await HttpApiClient.MakeApiPostRequestAsync<PropertyDto>($"{PropertyBuildingEnpoint.AddImage}",
            Is.EqualTo(HttpStatusCode.OK), newImages);

        var insertedEntity = await GetEntityWithApi<PropertyDto>($"{PropertyBuildingEnpoint.ById}/{result.Data.IdProperty}");
        //await GetEntityDto<Property, PropertyDto>(result.Data.IdProperty);
        Utilities.ValidateApiResult_ExpectedSuccess(result);

        Assert.That(result.Data.IdProperty, Is.GreaterThan(0));
        Assert.That(insertedEntity, Is.Not.Null);
        Assert.That(insertedEntity.PropertyImages.Count(), Is.EqualTo(initialImages + 1));
    }
    #endregion

    #region UPDATE_TESTS

    /// <summary>
    /// Test to verify that an OK response with property data is returned when updating a property with valid data (no images or traces).
    /// </summary>
    /// <param name="sameOwner">Indicates whether the property has the same owner as an existing property.</param>
    /// <param name="initialImages">The initial number of images associated with the property.</param>
    /// <param name="initialTraces">The initial number of traces associated with the property.</param>
    /// <param name="numImages">The number of images to associate with the property after the update.</param>
    /// <param name="numTraces">The number of traces to associate with the property after the update.</param>
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

        var result = await HttpApiClient.MakeApiPutRequestAsync<PropertyDto>($"{PropertyBuildingEnpoint.Update}",
            Is.EqualTo(HttpStatusCode.OK), expectedProperty);

        Utilities.ValidateApiResult_ExpectedSuccess(result);

        var updatedEntity = await GetEntityWithApi<PropertyDto>($"{PropertyBuildingEnpoint.ById}/{result.Data.IdProperty}");
        Assert.That(result.Data.IdProperty, Is.EqualTo(expectedProperty.IdProperty));

        Assert.That(expectedProperty.PropertyImages.Count(), Is.EqualTo(numImages));
        Assert.That(expectedProperty.PropertyTraces.Count(), Is.EqualTo(numTraces));
        Assert.That(expectedProperty.IdOwner, Is.EqualTo(updatedEntity.IdOwner));
        Assert.That(expectedProperty.Price, Is.EqualTo(updatedEntity.Price));
        Assert.That(expectedProperty.Name, Is.EqualTo(updatedEntity.Name));
        Assert.That(expectedProperty.Address, Is.EqualTo(updatedEntity.Address));
    }

    /// <summary>
    /// Test to verify that an OK response with property data is returned when updating the price of a property with a valid ID.
    /// </summary>
    public async Task Should_ReturnOkResponseWithPropertyData_When_UpdatePropertyPriceWithValidId()
    {
        decimal newPrice = 1419232.22M;
        var currentProperty = await this.InsertValidEntityDto<Property, PropertyDto>(PropertyBuildingDataFactory.GenerateRandomValidProperties(1, GetValidOwnerIdList(), 0, false, 4, 2).FirstOrDefault());

        var result = await HttpApiClient.MakeApiPutRequestAsync<PropertyDto>(
            $"{PropertyBuildingEnpoint.ChangePrice}/{currentProperty.IdProperty}?inNewPrice={newPrice}",
            Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedSuccess(result);

        var updatedEntity = await GetEntityWithApi<PropertyDto>($"{PropertyBuildingEnpoint.ById}/{result.Data.IdProperty}");

        Assert.That(result.Data.IdProperty, Is.EqualTo(updatedEntity.IdProperty));
        Assert.That(updatedEntity.Price, Is.EqualTo(newPrice));
    }
    #endregion

}