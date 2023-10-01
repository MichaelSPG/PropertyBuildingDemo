using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Tests.Factories;
using PropertyBuildingDemo.Tests.Helpers;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using PropertyBuildingDemo.Domain.Entities.Identity;
using PropertyBuildingDemo.Infrastructure.Data;
using NUnit.Framework.Constraints;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Domain.Interfaces;

namespace PropertyBuildingDemo.Tests.IntegrationTests.TestFixtures.OwnerTests;

[TestFixture]
public class OwnerCreationTests : BaseTest
{
    private UserRegisterDto _validUserRegistration;

    public List<OwnerDto> ValidOwnerList;

    private const int ValidOwnerCount = 10;

    async Task<Owner> GetOwnerDto(long id)
    {
        return await _unitOfWork.GetRepository<Owner>().GetAsync(id);
    }

    async Task<OwnerDto> InsertValidOwnerDto(OwnerDto ownerDto)
    {
        Owner owner = _mapper.Map<Owner>(ownerDto);
        await _unitOfWork.GetRepository<Owner>().AddAsync(owner);
        await _unitOfWork.Complete();
        return _mapper.Map<OwnerDto>(owner);
    }
    async Task<OwnerDto> InsertDeletedOwnerDto(OwnerDto ownerDto)
    {
        Owner owner = _mapper.Map<Owner>(ownerDto);
        owner.IsDeleted = true;
        await _unitOfWork.GetRepository<Owner>().AddAsync(owner);
        await _unitOfWork.Complete();
        return _mapper.Map<OwnerDto>(owner);
    }
    async Task<List<OwnerDto>> InsertListOfOwners(List<OwnerDto> list)
    {
        List<Owner> owners = _mapper.Map<List<Owner>>(list);
        await _unitOfWork.GetRepository<Owner>().AddRangeAsync(owners);
        await _unitOfWork.Complete();
        return _mapper.Map<List<OwnerDto>>(owners);
    }

    [SetUp]
    public async Task Setup()
    {
        _validUserRegistration = AccountUserDataFactory.CreateValidTestUserForRegister();
        await SetupUserDataAsync(_validUserRegistration);
        httpApiClient = CreateAuthorizedApiClient();

        ValidOwnerList = PropertyBuildingDataFactory.CreateValidTestOwnerList(ValidOwnerCount);
    }

    private async Task<List<OwnerDto>> GetOwnerListWithApi(bool expectsOkResult = true)
    {
        var result = await httpApiClient.MakeApiGetRequestAsync<List<OwnerDto>>($"{TestConstants.OwnerEnpoint.List}",
            expectsOkResult ? Is.EqualTo(HttpStatusCode.OK) : Is.Not.EqualTo(HttpStatusCode.OK));
        if (expectsOkResult)
            Utilities.ValidateApiResultData_ExpectedSuccess(result);
        else
            Utilities.ValidateApiResult_ExpectedFailed(result);
        return result.Data;
    }

    #region INSERT_TESTS

    [Test()]
    public async Task Should_ReturnOkResponseWithOwnerDataDifferentId_When_InsertOwnerWithCustomId()
    {
        long customId = Utilities.Random.Next(10000, int.MaxValue); // custom Id
        var expectedOwnerDto = PropertyBuildingDataFactory.CreateValidTestOwnerDto();
        expectedOwnerDto.IdOwner = customId;

        var result = await httpApiClient.MakeApiPostRequestAsync<OwnerDto>($"{TestConstants.OwnerEnpoint.Insert}", Is.EqualTo(HttpStatusCode.OK), expectedOwnerDto);

        Utilities.ValidateApiResult_ExpectedFailed(result);
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "identity_insert is set to OFF");
    }

    [Test()]
    public async Task Should_ReturnBadRequestResponse_When_InsertSingleOwnerWithInvalidBirthdayAgeRange()
    {
        var expectedOwnerDto = PropertyBuildingDataFactory.CreateValidTestOwnerDto();
        expectedOwnerDto.BirthDay = DateTime.Now.AddYears(-Utilities.Random.Next(1, 17));
        var result = await httpApiClient.MakeApiPostRequestAsync<OwnerDto>($"{TestConstants.OwnerEnpoint.Insert}", Is.EqualTo(HttpStatusCode.BadRequest), expectedOwnerDto);
        Utilities.ValidateApiResult_ExpectedFailed(result);
        Assert.IsNotNull(result.Message);
        Assert.That(result.Message.Count(), Is.EqualTo(1), $"Must only have one error {result.GetJoinedMessages()}");
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "valid age range");
    }

    [Test()]
    public async Task Should_ReturnBadRequestResponse_When_InsertSingleOwnerHasBirthDayInTheFuture()
    {
        var expectedOwnerDto = PropertyBuildingDataFactory.CreateValidTestOwnerDto();
        expectedOwnerDto.BirthDay = DateTime.Now.AddYears(Utilities.Random.Next(1, 17));
        var result = await httpApiClient.MakeApiPostRequestAsync<OwnerDto>($"{TestConstants.OwnerEnpoint.Insert}", Is.EqualTo(HttpStatusCode.BadRequest), expectedOwnerDto);
        Utilities.ValidateApiResult_ExpectedFailed(result);
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "cannot be in the future");
    }

    [Test()]
    public async Task Should_ReturnBadRequestResponse_When_SingleInsertSingleOwnerWithNullPhotoData()
    {
        var expectedOwnerDto = PropertyBuildingDataFactory.CreateValidTestOwnerDto();
        expectedOwnerDto.Photo = null;
        var result = await httpApiClient.MakeApiPostRequestAsync<OwnerDto>($"{TestConstants.OwnerEnpoint.Insert}", Is.EqualTo(HttpStatusCode.BadRequest), expectedOwnerDto);
        Utilities.ValidateApiResult_ExpectedFailed(result);
    }

    [Test]
    public async Task Should_ReturnOkResponseWithOwnerData_When_InsertMultipleValidOwnerData([Random(0, ValidOwnerCount, ValidOwnerCount)] int index)
    {
        OwnerDto owner = ValidOwnerList[index];
        var result = await httpApiClient.MakeApiPostRequestAsync<OwnerDto>($"{TestConstants.OwnerEnpoint.Insert}", Is.EqualTo(HttpStatusCode.OK), owner);

        Utilities.ValidateApiResultData_ExpectedSuccess(result);

        Assert.That(result.Data.IdOwner, Is.Not.Negative.And.GreaterThan(0));
        var resultOwnerDto = await GetOwnerDto(result.Data.IdOwner);
        Assert.IsNotNull(resultOwnerDto, "Owner not created in DB");
        Assert.That(result.Data.Photo.Length, Is.EqualTo(owner.Photo.Length));
    }
    [Test]
    public async Task Should_ReturnBadRequestResponse_When_InsertOwnerWithInvalidPhotoLenght()
    {
        var expectedOwnerDto = PropertyBuildingDataFactory.CreateRandomOwner();
        expectedOwnerDto.Photo = expectedOwnerDto.Photo.Take(199).ToArray();
        var result = await httpApiClient.MakeApiPostRequestAsync<OwnerDto>($"{TestConstants.OwnerEnpoint.Insert}", Is.EqualTo(HttpStatusCode.BadRequest), expectedOwnerDto);

        Utilities.ValidateApiResult_ExpectedFailed(result);
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "must have a minimum length");
    }

    #endregion

    #region UPDATE_TESTS

    [Test]
    public async Task Should_ReturnOkResponseWithOwnerData_When_UpdateMultipleOwnerWithValidData([Random(0, ValidOwnerCount, ValidOwnerCount)] int index)
    {
        var expectedOwnerDto = ValidOwnerList[index];
        expectedOwnerDto = await InsertValidOwnerDto(expectedOwnerDto);

        expectedOwnerDto.Photo = PropertyBuildingDataFactory.GenerateRandomByteArray();
        expectedOwnerDto.BirthDay = PropertyBuildingDataFactory.GenerateValidAgeRandomDate();
        expectedOwnerDto.Address = PropertyBuildingDataFactory.GenerateValidRandomAddress();

        var result = await httpApiClient.MakeApiPutRequestAsync<OwnerDto>($"{TestConstants.OwnerEnpoint.Update}", Is.EqualTo(HttpStatusCode.OK), expectedOwnerDto);

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
        var result = await httpApiClient.MakeApiGetRequestAsync<OwnerDto>($"{TestConstants.OwnerEnpoint.ById}/{id}",
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
        var result = await httpApiClient.MakeApiGetRequestAsync<OwnerDto>($"{TestConstants.OwnerEnpoint.ById}/{notExistentId}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedFailed(result);
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithOwnerData_When_SearchByIdWithValidOwnerId()
    {
        var expectedOwnerDto = ValidOwnerList.FirstOrDefault();
        expectedOwnerDto = await InsertValidOwnerDto(expectedOwnerDto);

        var result = await httpApiClient.MakeApiGetRequestAsync<OwnerDto>($"{TestConstants.OwnerEnpoint.ById}/{expectedOwnerDto.IdOwner}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResultData_ExpectedSuccess(result);
        Assert.That(result.Data.Name, Is.EqualTo(expectedOwnerDto.Name));
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithApiResultError_When_GetByIdDeleteOwner()
    {
        var deletedOwner = await InsertDeletedOwnerDto(PropertyBuildingDataFactory.CreateValidTestOwnerDto());

        var result = await httpApiClient.MakeApiGetRequestAsync<OwnerDto>($"{TestConstants.OwnerEnpoint.ById}/{deletedOwner.IdOwner}",
            Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedSuccessButNullData(result);

        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "not found");
    }

    #endregion

    #region LIST_TESTS

    [Test()]
    public async Task Should_ReturnOkResponseWithOwnerList_When_QueryAllFromApi()
    {
        var resultList = await InsertListOfOwners(ValidOwnerList);

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
            originList = await InsertListOfOwners(ValidOwnerList);
        }
        long idToDelete = Utilities.Random.Next(10000, int.MaxValue);
        var result = await httpApiClient.MakeApiDeleteRequestAsync<OwnerDto>($"{TestConstants.OwnerEnpoint.Delete}/{idToDelete}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedFailed(result);
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "does not exist");
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithOwnerListButOneIsDeleted_When_DeleteOwnerWithInvalidId()
    {
        var origintList = await GetOwnerListWithApi();
        if (!origintList.Any())
        {
            origintList = await InsertListOfOwners(ValidOwnerList);
        }
        long idToDelete = origintList[Utilities.Random.Next(origintList.Count)].IdOwner;
        var result = await httpApiClient.MakeApiDeleteRequestAsync<OwnerDto>($"{TestConstants.OwnerEnpoint.Delete}/{idToDelete}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResultData_ExpectedSuccess(result);
        var resultList = await GetOwnerListWithApi();
        Assert.IsNull(resultList.Find(x => x.IdOwner == idToDelete));

        Assert.That(origintList.Count, Is.GreaterThan(resultList.Count));
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithApiResultError_When_DeleteOwnerAlreadyDeleted()
    {
        var deletedOwner = await InsertDeletedOwnerDto(PropertyBuildingDataFactory.CreateValidTestOwnerDto());

        var result = await httpApiClient.MakeApiDeleteRequestAsync<OwnerDto>($"{TestConstants.OwnerEnpoint.Delete}/{deletedOwner.IdOwner}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedFailed(result);
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "does not exist");
    }

    #endregion

}