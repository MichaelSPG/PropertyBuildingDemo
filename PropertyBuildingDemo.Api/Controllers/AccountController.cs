using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Domain.Interfaces;
using PropertyBuildingDemo.Infrastructure.Data;
using PropertyBuildingDemo.Domain.Entities.Enums;
using TEntityBuildingDemo.Api.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PropertyBuildingDemo.Domain.Entities.Identity;
using IdentityModel;
using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using static Duende.IdentityServer.Models.IdentityResources;

namespace PropertyBuildingDemo.Api.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = "JwtClient")]
    public class AccountController : BaseController
    {
        private readonly ITokenService _tokenService;
        private readonly IUserAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly ISystemLogger _systemLogger;
        public AccountController( ISystemLogger systemLogger, ITokenService tokenService, IUserAccountService accountService)
        {
            _systemLogger = systemLogger;
            _tokenService = tokenService;
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] TokenRequest inTokenRequest)
        {
            ApiResult<TokenResponse> apiResult = null;
            try
            {
                apiResult = await _tokenService.CreateToken(inTokenRequest);
            }
            catch (Exception ex)
            {
                apiResult = ApiResult<TokenResponse>.FailedResult(ex.Message);
                _systemLogger.LogExceptionMessage(ELogginLevel.Level_Error, $"{nameof(TokenResponse)}/RequestToken failed!", ex);
            }

            return Ok(apiResult);
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> RegisterAsync(UserRegisterDto regsiterDto)
        {
            ApiResult<UserDto> apiResult = null;
            try
            {
                apiResult = await _accountService.RegisterUser(regsiterDto);
            }
            catch (Exception ex)
            {
                apiResult = ApiResult<UserDto>.FailedResult(ex.Message);
                _systemLogger.LogExceptionMessage(ELogginLevel.Level_Error, $"{nameof(UserDto)}/Register failed!", ex);
            }

            return Ok(apiResult);
        }
        [AllowAnonymous]
        [HttpGet("ExistsEmail")]
        public async Task<ActionResult<UserDto>> CheckEmailAsync([FromQuery] string Email)
        {
            ApiResult<UserDto> apiResult = null;
            try
            {
                apiResult = await _accountService.FindByEmail(Email);
            }
            catch (Exception ex)
            {
                apiResult = ApiResult<UserDto>.FailedResult(ex.Message);
                _systemLogger.LogExceptionMessage(ELogginLevel.Level_Error, $"{nameof(UserDto)}/CheckEmailAsync failed!", ex);
            }

            return Ok(apiResult);
        }

        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            ApiResult<UserDto> apiResult = null;
            try
            {
                apiResult = await _accountService.GetCurrentUser(HttpContext);
            }
            catch (Exception ex)
            {
                apiResult = ApiResult<UserDto>.FailedResult(ex.Message);
                _systemLogger.LogExceptionMessage(ELogginLevel.Level_Error, $"{nameof(UserDto)}/GetCurrentUser failed!", ex);
            }
            return Ok(apiResult);
        }
    }
}
