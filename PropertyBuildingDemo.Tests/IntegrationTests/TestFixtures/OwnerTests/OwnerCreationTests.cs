using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Tests.Factories;
using PropertyBuildingDemo.Tests.Helpers;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using PropertyBuildingDemo.Domain.Entities.Identity;
using PropertyBuildingDemo.Infrastructure.Data;
using NUnit.Framework.Constraints;

namespace PropertyBuildingDemo.Tests.IntegrationTests.TestFixtures.OwnerTests;

[TestFixture]
public class OwnerCreationTests : BaseTest
{
    private UserRegisterDto validUserRegistration;

    public List<OwnerDto> _validOwnerList;

    async Task<OwnerDto> GetOwnnerDto(long id)
    {
        return _mapper.Map< OwnerDto>(await _dbContext.Owner.FindAsync(id));
    }

    public static IEnumerable<OwnerDto> ValidOwnerTestCases()
    {
        
        foreach (var owner in PropertyBuildingDataFactory.CreateValidTestOwnerList(20))
        {
            yield return owner;
        }
    }


    [SetUp]
    public async Task Setup()
    {
        validUserRegistration = AccountUserDataFactory.CreateValidTestUserForRegister();
        await SetupUserDataAsync(validUserRegistration);
        httpApiClient = CreateAuthorizedApiClient();
    }

    [Test()]
    public async Task Should_ReturnOkResponseWithEmptyOwner_When_InvalidOwnerId()
    {
        int notExistentId = 3423442;
        var result = await httpApiClient.MakeApiGetRequestAsync<OwnerDto>($"{TestConstants.OwnerEnpoint.ById}?id={notExistentId}", Is.EqualTo(HttpStatusCode.OK));

        Utilities.ValidateApiResult_ExpectedNotOk(result);
    }

    [Test()]
    public async Task Should_ReturnBadRequestResponse_When_SingleOwnerHasInvalidBirthdayAgeRange()
    {
        var expectedOwnerDto = PropertyBuildingDataFactory.CreateValidTestOwnerDto();
        expectedOwnerDto.BirthDay =DateTime.Now.AddYears(-Utilities.Random.Next(1, 17));
        var result = await httpApiClient.MakeApiPostRequestAsync<OwnerDto>($"{TestConstants.OwnerEnpoint.Insert}", Is.EqualTo(HttpStatusCode.BadRequest), expectedOwnerDto);
        Utilities.ValidateApiResult_ExpectedNotOk(result);
        Assert.IsNotNull(result.Message);
        Assert.That(result.Message.Count(), Is.EqualTo(1), $"Must only have one error {result.GetJoinedMessages()}");
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "valid age range");
    }

    [Test()]
    public async Task Should_ReturnBadRequestResponse_When_SingleOwnerHasBirthDayInTheFuture()
    {
        var expectedOwnerDto = PropertyBuildingDataFactory.CreateValidTestOwnerDto();
        expectedOwnerDto.BirthDay = DateTime.Now.AddYears(Utilities.Random.Next(1, 17));
        var result = await httpApiClient.MakeApiPostRequestAsync<OwnerDto>($"{TestConstants.OwnerEnpoint.Insert}", Is.EqualTo(HttpStatusCode.BadRequest), expectedOwnerDto);
        Utilities.ValidateApiResult_ExpectedNotOk(result);
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "cannot be in the future");
    }

    [Test()]
    public async Task Should_ReturnBadRequestResponse_When_SingleOwnerHasNullPhotoData()
    {
        var expectedOwnerDto = PropertyBuildingDataFactory.CreateValidTestOwnerDto();
        expectedOwnerDto.Photo = null;
        var result = await httpApiClient.MakeApiPostRequestAsync<OwnerDto>($"{TestConstants.OwnerEnpoint.Insert}", Is.EqualTo(HttpStatusCode.BadRequest), expectedOwnerDto);
        Utilities.ValidateApiResult_ExpectedNotOk(result);
    }

    [Test, TestCaseSource(nameof(ValidOwnerTestCases))]
    public async Task Should_ReturnOkResponseWithOwnerData_When_MultipleOwnerIsValidData(OwnerDto owner)
    {
        var result = await httpApiClient.MakeApiPostRequestAsync<OwnerDto>($"{TestConstants.OwnerEnpoint.Insert}", Is.EqualTo(HttpStatusCode.OK), owner);

        Utilities.ValidateApiResultData(result);

        Assert.That(result.Data.IdOwner, Is.Not.Negative.And.GreaterThan(0));
        var resultOwnerDto = await GetOwnnerDto(result.Data.IdOwner);
        Assert.IsNotNull(resultOwnerDto, "Owner not created in DB");
        Assert.That(result.Data.Photo.Length, Is.EqualTo(owner.Photo.Length));
    }
    [Test]
    public async Task Should_ReturnBadRequestResponse_When_OwnerHasPhotoInvalidLenght()
    {
        var expectedOwnerDto = PropertyBuildingDataFactory.CreateRandomOwner();
        expectedOwnerDto.Photo = expectedOwnerDto.Photo.Take(199).ToArray();
        var result = await httpApiClient.MakeApiPostRequestAsync<OwnerDto>($"{TestConstants.OwnerEnpoint.Insert}", Is.EqualTo(HttpStatusCode.BadRequest), expectedOwnerDto);

        Utilities.ValidateApiResult_ExpectedNotOk(result);
        Utilities.ValidateApiResultMessage_ExpectContainsValue(result, "must have a minimum length");
    }

}