using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Tests.Factories;
using PropertyBuildingDemo.Tests.Helpers;
using System.Net;

namespace PropertyBuildingDemo.Tests.IntegrationTests.TestFixtures;

/// <summary>
/// Represents a test fixture for Property Images integration tests.
/// </summary>
[TestFixture]
public class PropertyImagesIntegrationTests : GenericIntegrationTest<PropertyImage, PropertyImageDto>
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

        ValidEntityList.ForEach(x => x.IdProperty = _propertyValidList.FirstOrDefault().IdProperty);
    }

    protected override void SetIdToEntity(long id, PropertyImageDto entity)
    {
        entity.IdPropertyImage = id;
    }

    protected override long GetEntityId(PropertyImageDto entity)
    {
        return entity.IdPropertyImage; 
    }


    private async Task<List<PropertyImageDto>> GetPropertyImageListWithApi(bool expectsOkResult = true)
    {
        var result = await HttpApiClient.MakeApiGetRequestAsync<List<PropertyImageDto>>($"{TestApiEndpoint.List}",
            expectsOkResult ? Is.EqualTo(HttpStatusCode.OK) : Is.Not.EqualTo(HttpStatusCode.OK));
        if (expectsOkResult)
            Utilities.ValidateApiResult_ExpectedSuccess(result);
        else
            Utilities.ValidateApiResult_ExpectedFailed(result);
        return result.Data;
    }

    #region INSERT_TESTS

    [Test()]
    public async Task Should_ReturnBadRequestResponse_When_SingleInsertSinglePropertyImageWithNullFileData()
    {
        var expectedPropertyImageDto = ValidTestEntityDto;
        expectedPropertyImageDto.File = null;
        var result = await HttpApiClient.MakeApiPostRequestAsync<PropertyImageDto>($"{TestApiEndpoint.Insert}", Is.EqualTo(HttpStatusCode.BadRequest), expectedPropertyImageDto);
        Utilities.ValidateApiResult_ExpectedFailed(result);
    }


    [Test]
    public async Task Should_ReturnBadRequestResponse_When_InsertPropertyImageWithInvalidFileLenght()
    {
        var expectedPropertyImageDto = DataFactory.CreateValidEntityDto();
        expectedPropertyImageDto.IdProperty = ValidTestEntityDto.IdProperty;

        expectedPropertyImageDto.File = expectedPropertyImageDto.File.Take(199).ToArray();
        var result = await HttpApiClient.MakeApiPostRequestAsync<PropertyImageDto>($"{TestApiEndpoint.Insert}", Is.EqualTo(HttpStatusCode.BadRequest), expectedPropertyImageDto);

        Utilities.ValidateApiResult_ExpectedFailed(result);
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "must have a minimum length");
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithEntityDataDifferentId_When_InsertEntityWithCustomId()
    {
        long customId = Utilities.Random.Next(10000, int.MaxValue); // custom Id
        var expectedEntityDto = DataFactory.CreateValidEntityDto();

        SetIdToEntity(customId, expectedEntityDto);

        var result = await HttpApiClient.MakeApiPostRequestAsync<PropertyImageDto>($"{TestApiEndpoint.Insert}", Is.EqualTo(HttpStatusCode.OK), expectedEntityDto);

        Utilities.ValidateApiResult_ExpectedFailed(result);
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "identity_insert is set to OFF");
    }

    [Test]
    public async Task Should_ReturnOkResponseWithEntityData_When_InsertMultipleValidEntityData([Random(0, ValidTestEntityCount, ValidTestEntityCount)] int index)
    {
        PropertyImageDto entity = ValidEntityList[index];
        var result = await HttpApiClient.MakeApiPostRequestAsync<PropertyImageDto>($"{TestApiEndpoint.Insert}", Is.EqualTo(HttpStatusCode.OK), entity);

        Utilities.ValidateApiResult_ExpectedSuccess(result);

        Assert.That(GetEntityId(result.Data), Is.Not.Negative.And.GreaterThan(0));
        var resultEntityDto = await GetEntityDto<PropertyImage, PropertyImageDto>(GetEntityId(result.Data));
        Assert.IsNotNull(resultEntityDto, $"{typeof(PropertyImage)} not created in DB");
    }

    #endregion

    #region UPDATE_TESTS

    [Test]
    public async Task Should_ReturnOkResponseWithPropertyImageData_When_UpdateMultiplePropertyImageWithValidData([Random(0, ValidTestEntityCount, ValidTestEntityCount)] int index)
    {
        var expectedPropertyImageDto = ValidEntityList[index];
        expectedPropertyImageDto = await InsertValidEntityDto<PropertyImage, PropertyImageDto>(expectedPropertyImageDto);

        expectedPropertyImageDto.File = Utilities.RandomGenerators.GenerateRandomByteArray();
        expectedPropertyImageDto.Enabled = false;
        expectedPropertyImageDto.IdProperty = 1;

        var result = await HttpApiClient.MakeApiPutRequestAsync<PropertyImageDto>($"{TestApiEndpoint.Update}", Is.EqualTo(HttpStatusCode.OK), expectedPropertyImageDto);

        Utilities.ValidateApiResult_ExpectedSuccess(result);

        Assert.That(result.Data.IdPropertyImage, Is.EqualTo(expectedPropertyImageDto.IdPropertyImage));

        var resultPropertyImageDto = await GetPropertyImageDtoWithApi(expectedPropertyImageDto.IdPropertyImage);

        Assert.IsNotNull(resultPropertyImageDto, "PropertyImage not created in DB");

        Assert.That(resultPropertyImageDto.File.Length, Is.EqualTo(expectedPropertyImageDto.File.Length));
        Assert.That(resultPropertyImageDto.Enabled, Is.EqualTo(expectedPropertyImageDto.Enabled));
        Assert.That(resultPropertyImageDto.IdProperty, Is.EqualTo(expectedPropertyImageDto.IdProperty));
    }

    #endregion

    #region GETBY_TESTS
    private async Task<PropertyImageDto> GetPropertyImageDtoWithApi(long id, bool expectsOkResult = true)
    {
        var result = await HttpApiClient.MakeApiGetRequestAsync<PropertyImageDto>($"{TestApiEndpoint.ById}/{id}",
            expectsOkResult ? Is.EqualTo(HttpStatusCode.OK) : Is.Not.EqualTo(HttpStatusCode.OK));
        if (expectsOkResult)
            Utilities.ValidateApiResult_ExpectedSuccess(result);
        else
            Utilities.ValidateApiResult_ExpectedFailed(result);
        return result.Data;
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithEmptyPropertyImage_When_SearchByIdWithInvalidPropertyImageId()
    {
        int notExistentId = 3423442;
        var result = await HttpApiClient.MakeApiGetRequestAsync<PropertyImageDto>($"{TestApiEndpoint.ById}/{notExistentId}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedSuccessButNullData(result);
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithPropertyImageData_When_SearchByIdWithValidPropertyImageId()
    {
        var expectedPropertyImageDto = ValidEntityList.FirstOrDefault();
        expectedPropertyImageDto = await InsertValidEntityDto<PropertyImage, PropertyImageDto>(expectedPropertyImageDto);

        var result = await HttpApiClient.MakeApiGetRequestAsync<PropertyImageDto>($"{TestApiEndpoint.ById}/{expectedPropertyImageDto.IdPropertyImage}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedSuccess(result);
        Assert.That(result.Data.IdPropertyImage, Is.EqualTo(expectedPropertyImageDto.IdPropertyImage));
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithApiResultError_When_GetByIdDeletePropertyImage()
    {
        var deletedPropertyImage = await InsertDeletedEntityDto<PropertyImage, PropertyImageDto>(ValidTestEntityDto);

        var result = await HttpApiClient.MakeApiGetRequestAsync<PropertyImageDto>($"{TestApiEndpoint.ById}/{deletedPropertyImage.IdPropertyImage}",
            Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedSuccessButNullData(result);

        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "not found");
    }

    #endregion

    #region LIST_TESTS

    [Test()]
    public async Task Should_ReturnOkResponseWithPropertyImageList_When_QueryAllFromApi()
    {
        var resultList = await InsertListOfEntity<PropertyImage, PropertyImageDto>(ValidEntityList);

        var result = await GetPropertyImageListWithApi();

        Assert.That(result.Count, Is.GreaterThanOrEqualTo(resultList.Count));
    }

    #endregion

    #region DELETE_TESTS

    [Test()]
    public async Task Should_ReturnOkResponseWithApiResultError_When_DeletePropertyImageWithNotExistentId()
    {
        var originList = await GetPropertyImageListWithApi();
        if (!originList.Any())
        {
            originList = await InsertListOfEntity<PropertyImage, PropertyImageDto>(ValidEntityList);
        }
        long idToDelete = Utilities.Random.Next(10000, int.MaxValue);
        var result = await HttpApiClient.MakeApiDeleteRequestAsync<PropertyImageDto>($"{TestApiEndpoint.Delete}/{idToDelete}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedFailed(result);
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "does not exist");
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithPropertyImageListButOneIsDeleted_When_DeletePropertyImageWithInvalidId()
    {
        var origintList = await GetPropertyImageListWithApi();
        if (!origintList.Any())
        {
            origintList = await InsertListOfEntity<PropertyImage, PropertyImageDto>(ValidEntityList);
        }
        long idToDelete = origintList[Utilities.Random.Next(origintList.Count)].IdPropertyImage;
        var result = await HttpApiClient.MakeApiDeleteRequestAsync<PropertyImageDto>($"{TestApiEndpoint.Delete}/{idToDelete}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedSuccess(result);
        var resultList = await GetPropertyImageListWithApi();
        Assert.IsNull(resultList.Find(x => x.IdPropertyImage == idToDelete));

        Assert.That(origintList.Count, Is.GreaterThan(resultList.Count));
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithApiResultError_When_DeletePropertyImageAlreadyDeleted()
    {
        var deletedPropertyImage = await InsertDeletedEntityDto<PropertyImage, PropertyImageDto>(ValidTestEntityDto);

        var result = await HttpApiClient.MakeApiDeleteRequestAsync<PropertyImageDto>($"{TestApiEndpoint.Delete}/{deletedPropertyImage.IdPropertyImage}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedFailed(result);
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "does not exist");
    }

    #endregion
}