﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Application.IServices;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities.Identity;
using PropertyBuildingDemo.Domain.Interfaces;
using PropertyBuildingDemo.Tests.Factories;
using PropertyBuildingDemo.Tests.Helpers;
using PropertyBuildingDemo.Tests.IntegrationTests.TestUtilities;
using System.Net;
using Microsoft.EntityFrameworkCore;
using PropertyBuildingDemo.Infrastructure.Data;
using static PropertyBuildingDemo.Tests.Helpers.TestConstants;
using IdentityModel;
using System;

namespace PropertyBuildingDemo.Tests.IntegrationTests
{
    /// <summary>
    /// Base class for test fixtures that require common setup and resources.
    /// </summary>
    [SetCulture("en-US")]
    public class BaseTest : IDisposable
    {
        /// <summary>
        /// Represents a valid user registration DTO used for authentication in tests.
        /// </summary>
        protected UserRegisterDto ValidUserRegistration;

        /// <summary>
        /// Represents the instance of the API web application factory used for integration testing.
        /// </summary>
        protected ApiWebApplicationFactory Application;

        /// <summary>
        /// Represents the token response obtained during authentication for testing secured endpoints.
        /// </summary>
        protected TokenResponse TokenResponse;

        /// <summary>
        /// Represents the user account service used for testing user-related functionalities.
        /// </summary>
        protected IUserAccountService UserAccountService;

        /// <summary>
        /// Represents a user DTO used for testing user-related functionalities.
        /// </summary>
        protected UserDto UserDto;

        /// <summary>
        /// Represents the HTTP API client used for making requests to the API during testing.
        /// </summary>
        protected HttpApiClient HttpApiClient;

        /// <summary>
        /// Represents the API token service used for handling API tokens during testing.
        /// </summary>
        protected IApiTokenService TokenService;

        /// <summary>
        /// Represents the unit of work used for database operations during testing.
        /// </summary>
        protected IUnitOfWork UnitOfWork;

        /// <summary>
        /// Represents the mapper used for mapping between DTOs and entities during testing.
        /// </summary>
        protected IMapper Mapper;

        protected int ValidTestEntityCount = 10;

        IServiceProvider _serviceProvider;

        /// <summary>
        /// Performs one-time setup for the test fixture, including creating the API web application factory and HTTP API client.
        /// </summary>
        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            Application = new ApiWebApplicationFactory();
            HttpApiClient = new HttpApiClient(Application.CreateClient(new WebApplicationFactoryClientOptions
            { AllowAutoRedirect = false }));
        }

        /// <summary>
        /// Performs cleanup and resource disposal after the test fixture has been used.
        /// </summary>
        public void Dispose()
        {
            HttpApiClient?.Dispose();
            Application?.Dispose();
        }

        /// <summary>
        /// Creates an HTTP API client with authorization token for making authorized requests to the API.
        /// </summary>
        /// <returns>An instance of <see cref="HttpApiClient"/> with the authorization token set.</returns>
        public HttpApiClient CreateAuthorizedApiClient()
        {
            var client = new HttpApiClient(Application.CreateClient(new WebApplicationFactoryClientOptions
            { AllowAutoRedirect = false }));
            client.SetTokenAuthorizationHeader(TokenResponse);
            return client;
        }

        public async Task SetupValidRegistrationUser()
        {
            ValidUserRegistration = AccountUserDataFactory.CreateValidTestUserForRegister();
            await SetupUserDataAsync(ValidUserRegistration);
            HttpApiClient = CreateAuthorizedApiClient();
        }

        /// <summary>
        /// Creates a user DTO based on the provided user registration DTO and registers the user in the system.
        /// </summary>
        /// <param name="userRegisterDto">The user registration DTO containing user information.</param>
        /// <returns>The created user DTO after registration.</returns>
        private async Task<UserDto> CreateTestUserDto(UserRegisterDto userRegisterDto)
        {
            var newUser = new AppUser
            {
                UserName = userRegisterDto.Email,
                DisplayName = userRegisterDto.DisplayName,
                Email = userRegisterDto.Email,
                IdentificationNumber = userRegisterDto.IdentificationNumber
            };
            var result = await UserAccountService.RegisterUser(userRegisterDto);

            return new UserDto()
            {
                DisplayName = newUser.DisplayName,
                Email = newUser.UserName,
                Id = newUser.Id,
                IdentificationNumber = newUser.IdentificationNumber,
            };
        }

        /// <summary>
        /// Retrieves a user DTO based on the provided user name by searching in the system.
        /// </summary>
        /// <param name="userName">The user name to search for in the system.</param>
        /// <returns>The found user DTO if it exists; otherwise, null.</returns>
        private async Task<UserDto> GetTestUserDto(string userName)
        {
            var result = await UserAccountService.FindByEmail(userName);
            if (result == null || result.Failed())
            {
                return null;
            }
            var appUser = result.Data;
            return new UserDto()
            {
                DisplayName = appUser.DisplayName,
                Email = appUser.Email,
                Id = appUser.Id,
                IdentificationNumber = appUser.IdentificationNumber,
            };
        }

        /// <summary>
        /// Creates a test user if it does not already exist and returns the user DTO.
        /// </summary>
        /// <param name="userRegisterDto">The user registration DTO containing user information.</param>
        /// <returns>The user DTO of the existing or newly created user.</returns>
        protected async Task<UserDto> CreateTestUserIfNotExists(UserRegisterDto userRegisterDto)
        {
            var user = await GetTestUserDto(userRegisterDto.Email);
            if (user != null)
            {
                return user;
            }

            return await CreateTestUserDto(userRegisterDto);
        }

        /// <summary>
        /// Creates a test token for the provided user using the specified API token service.
        /// </summary>
        /// <param name="tokenService">The API token service to use for creating the token.</param>
        /// <param name="userRegister">The user registration DTO for which to create the token.</param>
        /// <returns>The token response containing the created token data.</returns>
        private async Task<TokenResponse> CreateTestTokenForUser(IApiTokenService tokenService, UserRegisterDto userRegister)
        {
            var result = await tokenService.CreateToken(TokenDataFactory.CreateTokenRequestFromUserData(userRegister));
            return result.Data;
        }

        /// <summary>
        /// Sets up user-related data for testing purposes, including the user DTO, API token, and necessary services.
        /// </summary>
        /// <param name="userRegister">The user registration DTO containing user information.</param>
        /// <param name="scope">The service scope for resolving dependencies; if not provided, a new scope is created.</param>
        protected async Task SetupUserDataAsync(UserRegisterDto userRegister, IServiceScope scope = null)
        {
            scope ??= Application.Services.CreateScope();

            UnitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            TokenService = scope.ServiceProvider.GetRequiredService<IApiTokenService>();
            UserAccountService = scope.ServiceProvider.GetRequiredService<IUserAccountService>();
            Mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
            _serviceProvider = scope.ServiceProvider;
            UserDto = await CreateTestUserIfNotExists(userRegister);
            TokenResponse = await CreateTestTokenForUser(TokenService, userRegister);
        }


        /// <summary>
        /// Retrieves an entity by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>The entity object if found; otherwise, null.</returns>
        protected async Task<TEntityDto> GetEntityDto<TEntity, TEntityDto>(long id)
            where TEntity : BaseEntityDb
            where TEntityDto : class
        {
            return await GetDbEntityServices<TEntity, TEntityDto>().GetByIdAsync(id);
        }
        protected async Task<List<TEntityDto>> GetEntityList<TEntity, TEntityDto>()
            where TEntity : BaseEntityDb
            where TEntityDto : class
        {
            return await GetDbEntityServices<TEntity, TEntityDto>().GetAllAsync();
        }

        /// <summary>
        /// Inserts a valid entity DTO into the database and returns the corresponding DTO.
        /// </summary>
        /// <param name="entityDto">The entity DTO to insert.</param>
        /// <returns>The inserted entity DTO with the updated information.</returns>
        protected async Task<TEntityDto> InsertValidEntityDto<TEntity, TEntityDto>(TEntityDto entityDto)
            where TEntity : BaseEntityDb where TEntityDto : class
        {
            return await GetDbEntityServices<TEntity, TEntityDto>().AddAsync(entityDto);
        }

        /// <summary>
        /// Inserts an entity DTO into an API endpoint asynchronously.
        /// </summary>
        /// <typeparam name="TEntityDto">The DTO type representing the entity, typically a class.</typeparam>
        /// <param name="endpointUrl">The URL of the API endpoint to insert the entity into.</param>
        /// <param name="entityDto">The entity DTO to be inserted.</param>
        /// <param name="expectsOkResult">
        ///     A flag indicating whether an HTTP status code of OK (200) is expected as a result.
        ///     If set to true, the method will validate for a successful result; otherwise, it will validate for a failed result.
        /// </param>
        /// <returns>The entity DTO obtained from the API response after insertion.</returns>
        /// <remarks>
        /// If expectsOkResult is set to true, the method will validate the API result for a successful response using Utilities.ValidateApiResult_ExpectedSuccess.
        /// If expectsOkResult is set to false, the method will validate the API result for a failed response using Utilities.ValidateApiResult_ExpectedFailed.
        /// </remarks>
        protected async Task<TEntityDto> InsertEntityDtoWithApi<TEntityDto>(string endpointUrl, TEntityDto entityDto, bool expectsOkResult = true)
        {
            var result = await HttpApiClient.MakeApiPostRequestAsync<TEntityDto>(
                $"{endpointUrl}",
                expectsOkResult ? Is.EqualTo(HttpStatusCode.OK) : Is.Not.EqualTo(HttpStatusCode.OK), entityDto);

            if (expectsOkResult)
                Utilities.ValidateApiResult_ExpectedSuccess(result);
            else
                Utilities.ValidateApiResult_ExpectedFailed(result);

            return result.Data;
        }

        /// <summary>
        /// Inserts a list of entity DTOs into an API endpoint asynchronously.
        /// </summary>
        /// <typeparam name="TEntityDto">The DTO type representing the entity, typically a class.</typeparam>
        /// <param name="endpointUrl">The URL of the API endpoint to insert the entities into.</param>
        /// <param name="entitiesDto">The list of entity DTOs to be inserted.</param>
        /// <param name="expectsOkResult">
        ///     A flag indicating whether an HTTP status code of OK (200) is expected as a result for each insertion.
        ///     If set to true, the method will validate for a successful result for each insertion; otherwise, it will validate for a failed result.
        /// </param>
        /// <returns>The list of entity DTOs obtained from the API response after insertion.</returns>
        /// <remarks>
        /// If expectsOkResult is set to true, the method will validate the API result for a successful response using Utilities.ValidateApiResult_ExpectedSuccess for each insertion.
        /// If expectsOkResult is set to false, the method will validate the API result for a failed response using Utilities.ValidateApiResult_ExpectedFailed for each insertion.
        /// </remarks>
        protected async Task<List<TEntityDto>> InsertEntityDtoListWithApi<TEntityDto>(string endpointUrl, List<TEntityDto> entitiesDto, bool expectsOkResult = true)
        {
            for (int i = 0; i < entitiesDto.Count(); i++)
            {
                var result = await HttpApiClient.MakeApiPostRequestAsync<TEntityDto>(
                    $"{endpointUrl}",
                    expectsOkResult ? Is.EqualTo(HttpStatusCode.OK) : Is.Not.EqualTo(HttpStatusCode.OK), entitiesDto[i]);

                if (expectsOkResult)
                    Utilities.ValidateApiResult_ExpectedSuccess(result);
                else
                    Utilities.ValidateApiResult_ExpectedFailed(result);

                entitiesDto[i] = result.Data;
            }

            return entitiesDto;
        }

        /// <summary>
        /// Inserts a valid entity DTO with IsDeleted set to true into the database and returns the corresponding DTO.
        /// </summary>
        /// <param name="entityDto">The entity DTO to insert with IsDeleted set to true.</param>
        /// <returns>The inserted entity DTO with the updated information.</returns>
        public async Task<TEntityDto> InsertDeletedEntityDto<TEntity, TEntityDto>(TEntityDto entityDto)
            where TEntity : BaseEntityDb
            where TEntityDto : class
        {
            entityDto =  await InsertValidEntityDto<TEntity, TEntityDto>(entityDto);
            return await GetDbEntityServices<TEntity, TEntityDto>().DeleteAsync(entityDto);
        }

        /// <summary>
        /// Inserts a list of entity DTOs into the database and returns the corresponding DTOs.
        /// </summary>
        /// <param name="list">The list of entity DTOs to insert.</param>
        /// <returns>The inserted entity DTOs with the updated information.</returns>
        public async Task<List<TEntityDto>> InsertListOfEntity<TEntity, TEntityDto>(List<TEntityDto> list)
            where TEntity : BaseEntityDb
            where TEntityDto : class
        {
            var entities = Mapper.Map<List<TEntity>>(list);
            await UnitOfWork.GetRepository<TEntity>().AddRangeAsync(entities);
            await UnitOfWork.Complete();


            return Mapper.Map<List<TEntityDto>>(entities);
        }

        /// <summary>
        /// Retrieves a list of entity DTOs from the API and validates the result based on the expected HTTP status code.
        /// </summary>
        /// <param name="endpointUrl">The url of the endpoint</param>
        /// <param name="expectsOkResult">A flag indicating whether the result is expected to have an HTTP OK status code.</param>
        /// <returns>The list of entity DTOs retrieved from the API.</returns>
        public async Task<List<TEntityDto>> GetEntityListWithApi<TEntityDto>(string endpointUrl, bool expectsOkResult = true)
        {
            var result = await HttpApiClient.MakeApiGetRequestAsync<List<TEntityDto>>($"{endpointUrl}",
                expectsOkResult ? Is.EqualTo(HttpStatusCode.OK) : Is.Not.EqualTo(HttpStatusCode.OK));

            if (expectsOkResult)
                Utilities.ValidateApiResult_ExpectedSuccess(result);
            else
                Utilities.ValidateApiResult_ExpectedFailed(result);

            return result.Data;
        }

        /// <summary>
        /// Retrieves a service that provides database operations for a specific entity type and its associated DTO type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type derived from BaseEntityDb.</typeparam>
        /// <typeparam name="TEntityDto">The DTO type for the entity, typically a class.</typeparam>
        /// <returns>An instance of IDbEntityServices<TEntity, TEntityDto> obtained from the service provider.</returns>
        public IDbEntityServices<TEntity, TEntityDto> GetDbEntityServices<TEntity, TEntityDto>()
            where TEntity : BaseEntityDb
            where TEntityDto : class
        {
            return _serviceProvider.GetService<IDbEntityServices<TEntity, TEntityDto>>();
        }

        /// <summary>
        /// Makes an asynchronous API GET request to retrieve an entity using the provided endpoint URL.
        /// </summary>
        /// <typeparam name="TEntityDto">The DTO type representing the entity, typically a class.</typeparam>
        /// <param name="endpointUrl">The URL of the API endpoint to fetch the entity from.</param>
        /// <param name="expectsOkResult">
        ///     A flag indicating whether an HTTP status code of OK (200) is expected as a result.
        ///     If set to true, the method will validate for a successful result; otherwise, it will validate for a failed result.
        /// </param>
        /// <returns>The entity DTO obtained from the API response.</returns>
        /// <remarks>
        /// If expectsOkResult is set to true, the method will validate the API result for a successful response using Utilities.ValidateApiResult_ExpectedSuccess.
        /// If expectsOkResult is set to false, the method will validate the API result for a failed response using Utilities.ValidateApiResult_ExpectedFailed.
        /// </remarks>
        public async Task<TEntityDto> GetEntityWithApi<TEntityDto>(string endpointUrl, bool expectsOkResult = true)
        {
            var result = await HttpApiClient.MakeApiGetRequestAsync<TEntityDto>(
                $"{endpointUrl}",
                expectsOkResult ? Is.EqualTo(HttpStatusCode.OK) : Is.Not.EqualTo(HttpStatusCode.OK));

            if (expectsOkResult)
                Utilities.ValidateApiResult_ExpectedSuccess(result);
            else
                Utilities.ValidateApiResult_ExpectedFailed(result);

            return result.Data;
        }
    }
}
