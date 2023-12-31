using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Tests.Factories;
using PropertyBuildingDemo.Tests.Helpers;
using System.Net;

namespace PropertyBuildingDemo.Tests.IntegrationTests.TestFixtures;

/// <summary>
/// Represents a test fixture for owner integration tests.
/// </summary>
[TestFixture]
public class PropertyTraceIntegrationTests : GenericIntegrationTest<PropertyTrace, PropertyTraceDto>
{
    protected IDataFactory<OwnerDto> OwnerDataFactory;
    protected IDataFactory<PropertyDto> PropertyDataFactory;
    private List<PropertyDto> _propertyValidList;
    private List<OwnerDto> _ownerValidList;

    [SetUp]
    public async Task Setup()
    {
        await InitFactoryData();
        OwnerDataFactory = EntityDataFactoryManager.GetFactory<OwnerDto>();
        PropertyDataFactory = EntityDataFactoryManager.GetFactory<PropertyDto>();
        
        _ownerValidList = OwnerDataFactory.CreateValidEntityDtoList(ValidTestEntityCount).ToList();
        _ownerValidList = await InsertListOfEntity<Owner, OwnerDto>(_ownerValidList);
        _propertyValidList = PropertyDataFactory.CreateValidEntityDtoList(ValidTestEntityCount, _ownerValidList.FirstOrDefault().IdOwner).ToList();
        _propertyValidList = await InsertListOfEntity<Property, PropertyDto>(_propertyValidList);
        ValidTestEntityDto.IdProperty = _propertyValidList.FirstOrDefault().IdProperty;

        ValidEntityList.ForEach(x=> x.IdProperty = _propertyValidList.FirstOrDefault().IdProperty);

    }

    protected override void SetIdToEntity(long id, PropertyTraceDto entity)
    {
        entity.IdPropertyTrace = id;
    }

    protected override long GetEntityId(PropertyTraceDto entity)
    {
        return entity.IdPropertyTrace; 
    }

    #region INSERT_TESTS

    /// <summary>
    /// Test to verify that a bad request response is returned when inserting a single property trace with an invalid date of sale.
    /// </summary>
    [Test()]
    public async Task Should_ReturnBadRequestResponse_When_InsertSinglePropertyTraceWithInvalidDateSale()
    {
        var expectedPropertyTraceDto = ValidTestEntityDto;
        expectedPropertyTraceDto.DateSale = DateTime.Now.AddDays(Utilities.Random.Next(1, 20));
        var result = await HttpApiClient.MakeApiPostRequestAsync<PropertyTraceDto>($"{TestApiEndpoint.Insert}", Is.EqualTo(HttpStatusCode.BadRequest), expectedPropertyTraceDto);
        Utilities.ValidateApiResult_ExpectedFailed(result);
        Assert.IsNotNull(result.Message);
        Assert.That(result.Message.Count(), Is.EqualTo(1), $"Must only have one error {result.GetJoinedMessages()}");
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "must be in the past");
    }

    /// <summary>
    /// Test to verify that an OK response is returned when inserting an entity with a custom ID.
    /// </summary>
    [Test()]
    public async Task Should_ReturnOkResponseWithEntityDataDifferentId_When_InsertEntityWithCustomId()
    {
        long customId = Utilities.Random.Next(10000, int.MaxValue); // custom ID
        var expectedEntityDto = DataFactory.CreateValidEntityDto();

        SetIdToEntity(customId, expectedEntityDto);

        var result = await HttpApiClient.MakeApiPostRequestAsync<PropertyTraceDto>($"{TestApiEndpoint.Insert}", Is.EqualTo(HttpStatusCode.OK), expectedEntityDto);

        Utilities.ValidateApiResult_ExpectedFailed(result);
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "identity_insert is set to OFF");
    }

    /// <summary>
    /// Test to verify that an OK response is returned when inserting multiple valid entity data.
    /// </summary>
    [Test]
    public async Task Should_ReturnOkResponseWithEntityData_When_InsertMultipleValidEntityData([Random(0, ValidTestEntityCount, ValidTestEntityCount)] int index)
    {
        PropertyTraceDto entity = ValidEntityList[index];
        var result = await HttpApiClient.MakeApiPostRequestAsync<PropertyTraceDto>($"{TestApiEndpoint.Insert}", Is.EqualTo(HttpStatusCode.OK), entity);

        Utilities.ValidateApiResult_ExpectedSuccess(result);

        Assert.That(GetEntityId(result.Data), Is.Not.Negative.And.GreaterThan(0));
        var resultEntityDto = await GetEntityDto<PropertyTrace, PropertyTraceDto>(GetEntityId(result.Data));
        Assert.IsNotNull(resultEntityDto, $"{typeof(PropertyTraceDto)} not created in DB");
    }

    #endregion

    #region UPDATE_TESTS

    /// <summary>
    /// Test to verify that an OK response is returned when updating multiple PropertyTrace entities with valid data.
    /// </summary>
    [Test]
    public async Task Should_ReturnOkResponseWithPropertyTraceData_When_UpdateMultiplePropertyTraceWithValidData([Random(0, ValidTestEntityCount, ValidTestEntityCount)] int index)
    {
        var expectedPropertyTraceDto = ValidEntityList[index];
        expectedPropertyTraceDto = await InsertValidEntityDto<PropertyTrace, PropertyTraceDto>(expectedPropertyTraceDto);

        expectedPropertyTraceDto.Value = Utilities.RandomGenerators.GenerateRandomDecimal(100, 20000);
        expectedPropertyTraceDto.DateSale = Utilities.RandomGenerators.GenerateRandomDateInPast(20);
        expectedPropertyTraceDto.Tax = Utilities.RandomGenerators.GenerateRandomDecimal(100, 4500);
        expectedPropertyTraceDto.Name = Utilities.RandomGenerators.GenerateUniqueRandomName();
        expectedPropertyTraceDto.IdProperty = _propertyValidList.FirstOrDefault().IdProperty;

        var result = await HttpApiClient.MakeApiPutRequestAsync<PropertyTraceDto>($"{TestApiEndpoint.Update}", Is.EqualTo(HttpStatusCode.OK), expectedPropertyTraceDto);

        Utilities.ValidateApiResult_ExpectedSuccess(result);

        Assert.That(result.Data.IdPropertyTrace, Is.EqualTo(expectedPropertyTraceDto.IdPropertyTrace));

        var resultPropertyTraceDto = await GetPropertyTraceDtoWithApi(expectedPropertyTraceDto.IdPropertyTrace);

        Assert.IsNotNull(resultPropertyTraceDto, "PropertyTrace not created in DB");

        Assert.That(resultPropertyTraceDto.DateSale, Is.EqualTo(expectedPropertyTraceDto.DateSale));
        Assert.That(resultPropertyTraceDto.Tax, Is.EqualTo(expectedPropertyTraceDto.Tax));
        Assert.That(resultPropertyTraceDto.Name, Is.EqualTo(expectedPropertyTraceDto.Name));
        Assert.That(resultPropertyTraceDto.IdProperty, Is.EqualTo(expectedPropertyTraceDto.IdProperty));
    }

    #endregion

    #region GETBY_TESTS
    /// <summary>
    /// Gets a PropertyTraceDto with the specified ID from the API.
    /// </summary>
    /// <param name="id">The ID of the PropertyTrace to retrieve.</param>
    /// <param name="expectsOkResult">Specifies whether an OK result is expected.</param>
    /// <returns>The PropertyTraceDto obtained from the API.</returns>
    private async Task<PropertyTraceDto> GetPropertyTraceDtoWithApi(long id, bool expectsOkResult = true)
    {
        var result = await HttpApiClient.MakeApiGetRequestAsync<PropertyTraceDto>($"{TestApiEndpoint.ById}/{id}",
            expectsOkResult ? Is.EqualTo(HttpStatusCode.OK) : Is.Not.EqualTo(HttpStatusCode.OK));
        if (expectsOkResult)
            Utilities.ValidateApiResult_ExpectedSuccess(result);
        else
            Utilities.ValidateApiResult_ExpectedFailed(result);
        return result.Data;
    }

    /// <summary>
    /// Test to verify that an OK response is returned when searching by an invalid PropertyTrace ID.
    /// </summary>
    [Test()]
    public async Task Should_ReturnOkResponseWithEmptyPropertyTrace_When_SearchByIdWithInvalidPropertyTraceId()
    {
        int notExistentId = 3423442;
        var result = await HttpApiClient.MakeApiGetRequestAsync<PropertyTraceDto>($"{TestApiEndpoint.ById}/{notExistentId}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedSuccessButNullData(result);
    }

    /// <summary>
    /// Test to verify that an OK response with PropertyTrace data is returned when searching by a valid PropertyTrace ID.
    /// </summary>
    [Test()]
    public async Task Should_ReturnOkResponseWithPropertyTraceData_When_SearchByIdWithValidPropertyTraceId()
    {
        var expectedPropertyTraceDto = ValidEntityList.FirstOrDefault();
        expectedPropertyTraceDto = await InsertValidEntityDto<PropertyTrace, PropertyTraceDto>(expectedPropertyTraceDto);

        var result = await HttpApiClient.MakeApiGetRequestAsync<PropertyTraceDto>($"{TestApiEndpoint.ById}/{expectedPropertyTraceDto.IdPropertyTrace}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedSuccess(result);
        Assert.That(result.Data.IdPropertyTrace, Is.EqualTo(expectedPropertyTraceDto.IdPropertyTrace));
    }

    /// <summary>
    /// Test to verify that an OK response with API result error is returned when getting a deleted PropertyTrace by ID.
    /// </summary>
    [Test()]
    public async Task Should_ReturnOkResponseWithApiResultError_When_GetByIdDeletePropertyTrace()
    {
        var deletedPropertyTrace = await InsertDeletedEntityDto<PropertyTrace, PropertyTraceDto>(ValidTestEntityDto);

        var result = await HttpApiClient.MakeApiGetRequestAsync<PropertyTraceDto>($"{TestApiEndpoint.ById}/{deletedPropertyTrace.IdPropertyTrace}",
            Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedSuccessButNullData(result);

        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "not found");
    }
    #endregion

    #region LIST_TESTS

    [Test()]
    public async Task Should_ReturnOkResponseWithPropertyTraceList_When_QueryAllFromApi()
    {
        var resultList = await InsertListOfEntity<PropertyTrace, PropertyTraceDto>(ValidEntityList);

        var result = await GetEntityListWithApi<PropertyTraceDto>(TestApiEndpoint.List);

        Assert.That(result.Count, Is.GreaterThanOrEqualTo(resultList.Count));
    }

    #endregion

    #region DELETE_TESTS

    [Test()]
    public async Task Should_ReturnOkResponseWithApiResultError_When_DeletePropertyTraceWithNotExistentId()
    {
        var originList = await GetEntityListWithApi<PropertyTraceDto>(TestApiEndpoint.List);
        if (!originList.Any())
        {
            originList = await InsertListOfEntity<PropertyTrace, PropertyTraceDto>(ValidEntityList);
        }
        long idToDelete = Utilities.Random.Next(10000, int.MaxValue);
        var result = await HttpApiClient.MakeApiDeleteRequestAsync<PropertyTraceDto>($"{TestApiEndpoint.Delete}/{idToDelete}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedFailed(result);
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "does not exist");
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithPropertyTraceListButOneIsDeleted_When_DeletePropertyTraceWithInvalidId()
    {
        var originList = await GetEntityListWithApi< PropertyTraceDto>(TestApiEndpoint.List);
        if (!originList.Any())
        {
            originList = await InsertListOfEntity<PropertyTrace, PropertyTraceDto>(ValidEntityList);
        }
        long idToDelete = originList[Utilities.Random.Next(originList.Count)].IdPropertyTrace;
        var result = await HttpApiClient.MakeApiDeleteRequestAsync<PropertyTraceDto>($"{TestApiEndpoint.Delete}/{idToDelete}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedSuccess(result);
        var resultList = await GetEntityListWithApi<PropertyTraceDto>(TestApiEndpoint.List);
        Assert.IsNull(resultList.Find(x => x.IdPropertyTrace == idToDelete));

        Assert.That(originList.Count, Is.GreaterThan(resultList.Count));
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithApiResultError_When_DeletePropertyTraceAlreadyDeleted()
    {
        var deletedPropertyTrace = await InsertDeletedEntityDto<PropertyTrace, PropertyTraceDto>(ValidTestEntityDto);

        var result = await HttpApiClient.MakeApiDeleteRequestAsync<PropertyTraceDto>($"{TestApiEndpoint.Delete}/{deletedPropertyTrace.IdPropertyTrace}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedFailed(result);
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "does not exist");
    }

    #endregion
}