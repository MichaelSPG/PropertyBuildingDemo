using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Tests.Helpers;
using System.Net;

namespace PropertyBuildingDemo.Tests.IntegrationTests.TestFixtures;

[TestFixture]
public class OwnerIntegrationTests : GenericIntegrationTest<Owner, OwnerDto>
{
    [SetUp]
    public async Task Setup()
    {
        await InitFactoryData();
    }

    private async Task<List<OwnerDto>> GetOwnerListWithApi(bool expectsOkResult = true)
    {
        var result = await HttpApiClient.MakeApiGetRequestAsync<List<OwnerDto>>($"{TestApiEndpoint.List}",
            expectsOkResult ? Is.EqualTo(HttpStatusCode.OK) : Is.Not.EqualTo(HttpStatusCode.OK));
        if (expectsOkResult)
            Utilities.ValidateApiResultData_ExpectedSuccess(result);
        else
            Utilities.ValidateApiResult_ExpectedFailed(result);
        return result.Data;
    }

    #region INSERT_TESTS

    [Test()]
    public async Task Should_ReturnOkResponseWithEntityDataDifferentId_When_InsertEntityWithCustomId()
    {
        long customId = Utilities.Random.Next(10000, int.MaxValue); // custom Id
        var expectedEntityDto = DataFactory.CreateValidEntityDto();

        SetIdToEntity(customId, expectedEntityDto);

        var result = await HttpApiClient.MakeApiPostRequestAsync<OwnerDto>($"{TestApiEndpoint.Insert}", Is.EqualTo(HttpStatusCode.OK), expectedEntityDto);

        Utilities.ValidateApiResult_ExpectedFailed(result);
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "identity_insert is set to OFF");
    }

    [Test]
    public async Task Should_ReturnOkResponseWithEntityData_When_InsertMultipleValidEntityData([Random(0, ValidTestEntityCount, ValidTestEntityCount)] int index)
    {
        var entity = ValidEntityList[index];
        var result = await HttpApiClient.MakeApiPostRequestAsync<OwnerDto>($"{TestApiEndpoint.Insert}", Is.EqualTo(HttpStatusCode.OK), entity);

        Utilities.ValidateApiResultData_ExpectedSuccess(result);

        Assert.That(GetEntityId(result.Data), Is.Not.Negative.And.GreaterThan(0));
        var resultEntityDto = await GetEntityDto<Owner, OwnerDto>(GetEntityId(result.Data));
        Assert.IsNotNull(resultEntityDto, $"{typeof(OwnerDto)} not created in DB");
    }

    [Test()]
    public async Task Should_ReturnBadRequestResponse_When_InsertSingleOwnerWithInvalidBirthdayAgeRange()
    {
        var expectedOwnerDto = ValidTestEntityDto;
        expectedOwnerDto.BirthDay = DateTime.Now.AddYears(-Utilities.Random.Next(1, 17));
        var result = await HttpApiClient.MakeApiPostRequestAsync<OwnerDto>($"{TestApiEndpoint.Insert}", Is.EqualTo(HttpStatusCode.BadRequest), expectedOwnerDto);
        Utilities.ValidateApiResult_ExpectedFailed(result);
        Assert.IsNotNull(result.Message);
        Assert.That(result.Message.Count(), Is.EqualTo(1), $"Must only have one error {result.GetJoinedMessages()}");
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "valid age range");
    }

    [Test()]
    public async Task Should_ReturnBadRequestResponse_When_InsertSingleOwnerHasBirthDayInTheFuture()
    {
        var expectedOwnerDto = ValidTestEntityDto;
        expectedOwnerDto.BirthDay = DateTime.Now.AddYears(Utilities.Random.Next(1, 17));
        var result = await HttpApiClient.MakeApiPostRequestAsync<OwnerDto>($"{TestApiEndpoint.Insert}", Is.EqualTo(HttpStatusCode.BadRequest), expectedOwnerDto);
        Utilities.ValidateApiResult_ExpectedFailed(result);
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "must be in the past");
    }

    [Test()]
    public async Task Should_ReturnBadRequestResponse_When_SingleInsertSingleOwnerWithNullPhotoData()
    {
        var expectedOwnerDto = ValidTestEntityDto;
        expectedOwnerDto.Photo = null;
        var result = await HttpApiClient.MakeApiPostRequestAsync<OwnerDto>($"{TestApiEndpoint.Insert}", Is.EqualTo(HttpStatusCode.BadRequest), expectedOwnerDto);
        Utilities.ValidateApiResult_ExpectedFailed(result);
    }

    
    [Test]
    public async Task Should_ReturnBadRequestResponse_When_InsertOwnerWithInvalidPhotoLenght()
    {
        var expectedOwnerDto = DataFactory.CreateValidEntityDto();
        expectedOwnerDto.Photo = expectedOwnerDto.Photo.Take(199).ToArray();
        var result = await HttpApiClient.MakeApiPostRequestAsync<OwnerDto>($"{TestApiEndpoint.Insert}", Is.EqualTo(HttpStatusCode.BadRequest), expectedOwnerDto);

        Utilities.ValidateApiResult_ExpectedFailed(result);
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "must have a minimum length");
    }

    #endregion

    #region UPDATE_TESTS

    [Test]
    public async Task Should_ReturnOkResponseWithOwnerData_When_UpdateMultipleOwnerWithValidData([Random(0, ValidTestEntityCount, ValidTestEntityCount)] int index)
    {
        var expectedOwnerDto = ValidEntityList[index];
        expectedOwnerDto = await InsertValidEntityDto<Owner, OwnerDto>(expectedOwnerDto);

        expectedOwnerDto.Photo = Utilities.RandomGenerators.GenerateRandomByteArray();
        expectedOwnerDto.BirthDay = Utilities.RandomGenerators.GenerateValidAgeRandomDate();
        expectedOwnerDto.Address = Utilities.RandomGenerators.GenerateValidRandomAddress();

        var result = await HttpApiClient.MakeApiPutRequestAsync<OwnerDto>($"{TestApiEndpoint.Update}", Is.EqualTo(HttpStatusCode.OK), expectedOwnerDto);

        Utilities.ValidateApiResultData_ExpectedSuccess(result);

        Assert.That(result.Data.IdOwner, Is.EqualTo(expectedOwnerDto.IdOwner));

        var resultOwnerDto = await GetOwnerDtoWithApi(expectedOwnerDto.IdOwner);

        Assert.IsNotNull(resultOwnerDto, "Owner not created in DB");

        Assert.That(resultOwnerDto.Photo.Length, Is.EqualTo(expectedOwnerDto.Photo.Length));
        Assert.That(resultOwnerDto.Name, Is.EqualTo(expectedOwnerDto.Name));
        Assert.That(resultOwnerDto.Address, Is.EqualTo(expectedOwnerDto.Address));
        Assert.That(resultOwnerDto.BirthDay, Is.EqualTo(expectedOwnerDto.BirthDay));
    }

    #endregion

    #region GETBY_TESTS
    private async Task<OwnerDto> GetOwnerDtoWithApi(long id, bool expectsOkResult = true)
    {
        var result = await HttpApiClient.MakeApiGetRequestAsync<OwnerDto>($"{TestApiEndpoint.ById}/{id}",
            expectsOkResult ? Is.EqualTo(HttpStatusCode.OK) : Is.Not.EqualTo(HttpStatusCode.OK));
        if (expectsOkResult)
            Utilities.ValidateApiResultData_ExpectedSuccess(result);
        else
            Utilities.ValidateApiResult_ExpectedFailed(result);
        return result.Data;
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithEmptyOwner_When_SearchByIdWithInvalidOwnerId()
    {
        int notExistentId = 3423442;
        var result = await HttpApiClient.MakeApiGetRequestAsync<OwnerDto>($"{TestApiEndpoint.ById}/{notExistentId}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedSuccessButNullData(result);
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithOwnerData_When_SearchByIdWithValidOwnerId()
    {
        var expectedOwnerDto = ValidEntityList.FirstOrDefault();
        expectedOwnerDto = await InsertValidEntityDto<Owner, OwnerDto>(expectedOwnerDto);

        var result = await HttpApiClient.MakeApiGetRequestAsync<OwnerDto>($"{TestApiEndpoint.ById}/{expectedOwnerDto.IdOwner}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResultData_ExpectedSuccess(result);
        Assert.That(result.Data.Name, Is.EqualTo(expectedOwnerDto.Name));
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithApiResultError_When_GetByIdDeleteOwner()
    {
        var deletedOwner = await InsertDeletedEntityDto<Owner, OwnerDto>(ValidTestEntityDto);

        var result = await HttpApiClient.MakeApiGetRequestAsync<OwnerDto>($"{TestApiEndpoint.ById}/{deletedOwner.IdOwner}",
            Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedSuccessButNullData(result);

        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "not found");
    }

    #endregion

    #region LIST_TESTS

    [Test()]
    public async Task Should_ReturnOkResponseWithOwnerList_When_QueryAllFromApi()
    {
        var resultList = await InsertEntityDtoListWithApi<OwnerDto>(TestApiEndpoint.Insert, ValidEntityList);

        var result = await GetOwnerListWithApi();

        Assert.That(result.Count, Is.GreaterThanOrEqualTo(resultList.Count));
    }

    #endregion

    #region DELETE_TESTS

    [Test()]
    public async Task Should_ReturnOkResponseWithApiResultError_When_DeleteOwnerWithNotExistentId()
    {
        var originList = await GetOwnerListWithApi();
        if (!originList.Any())
        {
            originList = await InsertListOfEntity<Owner, OwnerDto>(ValidEntityList);
        }
        long idToDelete = Utilities.Random.Next(10000, int.MaxValue);
        var result = await HttpApiClient.MakeApiDeleteRequestAsync<OwnerDto>($"{TestApiEndpoint.Delete}/{idToDelete}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedFailed(result);
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "does not exist");
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithOwnerListButOneIsDeleted_When_DeleteOwnerWithInvalidId()
    {
        var origintList = await GetOwnerListWithApi();
        if (!origintList.Any())
        {
            origintList = await InsertListOfEntity<Owner, OwnerDto>(ValidEntityList);
        }
        long idToDelete = origintList[Utilities.Random.Next(origintList.Count)].IdOwner;
        var result = await HttpApiClient.MakeApiDeleteRequestAsync<OwnerDto>($"{TestApiEndpoint.Delete}/{idToDelete}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResultData_ExpectedSuccess(result);
        var resultList = await GetOwnerListWithApi();
        Assert.IsNull(resultList.Find(x => x.IdOwner == idToDelete));

        Assert.That(origintList.Count, Is.GreaterThan(resultList.Count));
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithApiResultError_When_DeleteOwnerAlreadyDeleted()
    {
        var deletedOwner = await InsertDeletedEntityDto<Owner, OwnerDto>(ValidTestEntityDto);

        var result = await HttpApiClient.MakeApiDeleteRequestAsync<OwnerDto>($"{TestApiEndpoint.Delete}/{deletedOwner.IdOwner}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedFailed(result);
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "does not exist");
    }

    #endregion

    protected override void SetIdToEntity(long id, OwnerDto entity)
    {
        entity.IdOwner = id;
    }

    protected override long GetEntityId(OwnerDto entity)
    {
        return entity.IdOwner;
    }
}