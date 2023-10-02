using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Tests.Factories;
using PropertyBuildingDemo.Tests.Helpers;
using System.Net;
using static PropertyBuildingDemo.Tests.Helpers.Helpers;

namespace PropertyBuildingDemo.Tests.IntegrationTests.TestFixtures
{
    /// <summary>
    /// Base class for integration tests of generic entities. It provides common functionality for testing
    /// CRUD operations on entities using API endpoints.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity being tested.</typeparam>
    /// <typeparam name="TEntityDto">The DTO (Data Transfer Object) type for the entity.</typeparam>
    public abstract class GenericIntegrationTest<TEntity, TEntityDto> : BaseTest where TEntity : BaseEntityDb
    {
        /// <summary>
        /// Represents a list of valid entity DTOs used for testing data.
        /// </summary>
        protected List<TEntityDto> ValidEntityList;

        /// <summary>
        /// Represents a data factory for generating entity DTOs.
        /// </summary>
        protected IDataFactory<TEntityDto> DataFactory;

        /// <summary>
        /// Represents a valid test entity DTO used for testing individual entities.
        /// </summary>
        protected TEntityDto ValidTestEntityDto;

        /// <summary>
        /// The count of valid test entities created for testing.
        /// </summary>
        protected const int ValidTestEntityCount = 10;

        /// <summary>
        /// The API endpoint URL for the entity being tested.
        /// </summary>
        protected TestConstants.IEndpointUrl TestApiEndpoint;

        /// <summary>
        /// Sets the ID of an entity in its DTO.
        /// </summary>
        /// <param name="id">The ID to set.</param>
        /// <param name="entity">The entity DTO.</param>
        protected abstract void SetIdToEntity(long id, TEntityDto entity);

        /// <summary>
        /// Gets the ID of an entity from its DTO.
        /// </summary>
        /// <param name="entity">The entity DTO.</param>
        /// <returns>The ID of the entity.</returns>
        protected abstract long GetEntityId(TEntityDto entity);

        /// <summary>
        /// Initializes the data factory, generates valid entity DTOs, and sets up user authentication for testing.
        /// </summary>
        protected async Task InitFactoryData()
        {
            DataFactory = EntityDataFactory.GetFactory<TEntityDto>();
            ValidEntityList = DataFactory.CreateValidEntityDtoList(ValidTestEntityCount).ToList();
            ValidTestEntityDto = DataFactory.CreateValidEntityDto();

            TestApiEndpoint = TestEndpoint.GetEndpoint<TEntityDto>();

            ValidUserRegistration = AccountUserDataFactory.CreateValidTestUserForRegister();
            await SetupUserDataAsync(ValidUserRegistration);
            HttpApiClient = CreateAuthorizedApiClient();
        }

        /// <summary>
        /// Retrieves a list of entity DTOs from the API and validates the result based on the expected HTTP status code.
        /// </summary>
        /// <param name="expectsOkResult">A flag indicating whether the result is expected to have an HTTP OK status code.</param>
        /// <returns>The list of entity DTOs retrieved from the API.</returns>
        protected async Task<List<TEntityDto>> GetEntityListWithApi(bool expectsOkResult = true)
        {
            var result = await HttpApiClient.MakeApiGetRequestAsync<List<TEntityDto>>($"{TestApiEndpoint.List}",
                expectsOkResult ? Is.EqualTo(HttpStatusCode.OK) : Is.Not.EqualTo(HttpStatusCode.OK));

            if (expectsOkResult)
                Utilities.ValidateApiResultData_ExpectedSuccess(result);
            else
                Utilities.ValidateApiResult_ExpectedFailed(result);

            return result.Data;
        }
    }
}
