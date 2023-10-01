using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using PropertyBuildingDemo.Domain.Entities.Identity;
using PropertyBuildingDemo.Tests.IntegrationTests.TestUtilities;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Interfaces;
using PropertyBuildingDemo.Tests.Factories;
using PropertyBuildingDemo.Tests.Helpers;
using System.Net;
using AutoMapper;
using PropertyBuildingDemo.Application.IServices;
using static System.Net.Mime.MediaTypeNames;
using PropertyBuildingDemo.Infrastructure.Data;

namespace PropertyBuildingDemo.Tests.IntegrationTests
{
    public class BaseTest : IDisposable
    {
        protected ApiWebApplicationFactory Application;
        protected TokenResponse _tokenResponse;
        protected IUserAccountService _userAccountService;
        protected UserDto _userDto;
        protected HttpApiClient httpApiClient;
        protected IApiTokenService _tokenService;
        protected PropertyBuildingContext _dbContext;
        protected IMapper _mapper;

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            Application = new ApiWebApplicationFactory();
            httpApiClient = new HttpApiClient(Application.CreateClient(new WebApplicationFactoryClientOptions
                { AllowAutoRedirect = false }));
        }

        public HttpApiClient CreateAuthorizedApiClient()
        {
            var client = new HttpApiClient(Application.CreateClient(new WebApplicationFactoryClientOptions
                { AllowAutoRedirect = false }));
            client.SetTokenAuthorizationHeader(_tokenResponse);
            return client;
        }

        public void Dispose()
        {
            httpApiClient?.Dispose();
            Application?.Dispose();
            _dbContext?.Dispose();
        }
        
        private async Task<UserDto> CreateTestUserDto(UserRegisterDto userRegisterDto)
        {
            var newUser = new AppUser
            {
                UserName = userRegisterDto.Email,
                DisplayName = userRegisterDto.DisplayName,
                Email = userRegisterDto.Email,
                IdentificationNumber = userRegisterDto.IdentificationNumber
            };
            var result = await _userAccountService.RegisterUser(userRegisterDto);

            return new UserDto()
            {
                DisplayName = newUser.DisplayName,
                Email = newUser.UserName,
                Id = newUser.Id,
                IdentificationNumber = newUser.IdentificationNumber,
            };
        }

        private async Task<UserDto> GetTestUserDto(string userName)
        {
            var result = await _userAccountService.FindByEmail(userName);
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

        public async Task<UserDto> CreateTestUserIfNotExists(UserRegisterDto userRegisterDto)
        {
            var user = await GetTestUserDto(userRegisterDto.Email);
            if (user != null)
            {
                return user;
            }

            return await CreateTestUserDto(userRegisterDto);
        }

        private async Task<TokenResponse> CreateTestTokenForUser(IApiTokenService tokenService, UserRegisterDto userRegister)
        {
            var result = await tokenService.CreateToken(TokenDataFactory.CreateTokenRequestFromUserData(userRegister));
            return result.Data;
        }

        public async Task SetupUserDataAsync(UserRegisterDto userRegister, IServiceScope scope = null)
        {
            if(scope == null)
                scope = Application.Services.CreateScope();

            _dbContext = scope.ServiceProvider.GetRequiredService<PropertyBuildingContext>();
            _tokenService = scope.ServiceProvider.GetRequiredService<IApiTokenService>();
            _userAccountService = scope.ServiceProvider.GetRequiredService<IUserAccountService>();
            _mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

            _userDto = await CreateTestUserIfNotExists(userRegister);
            _tokenResponse = await CreateTestTokenForUser(_tokenService, userRegister);

            
        }
    }
}
