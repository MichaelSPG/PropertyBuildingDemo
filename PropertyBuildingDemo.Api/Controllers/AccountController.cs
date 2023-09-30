// <copyright file="AccountController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace PropertyBuildingDemo.Api.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyBuildingDemo.Api.Filters;
using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Application.IServices;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities.Enums;
using PropertyBuildingDemo.Domain.Entities.Identity;
using PropertyBuildingDemo.Domain.Interfaces;

/// <summary>
/// Controller responsible for user account-related actions.
/// </summary>
[ApiVersion("1.0")]
[Authorize(AuthenticationSchemes = "JwtClient")]
public class AccountController : BaseController
{
    private readonly ITokenService _tokenService; // Service for token management
    private readonly IUserAccountService _accountService; // Service for user account operations
    private readonly ISystemLogger _systemLogger; // Logger for logging exceptions

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountController"/> class.
    /// </summary>
    /// <param name="systemLogger">The system logger for logging exceptions.</param>
    /// <param name="tokenService">The service for token management.</param>
    /// <param name="accountService">The service for user account operations.</param>
    public AccountController(ISystemLogger systemLogger, ITokenService tokenService, IUserAccountService accountService)
    {
        this._systemLogger = systemLogger;
        this._tokenService = tokenService;
        this._accountService = accountService;
    }

    /// <summary>
    /// Allows users to log in and obtain an authentication token.
    /// </summary>
    /// <param name="inTokenRequest">The token request data.</param>
    /// <returns>An IActionResult containing the token response or an error message.</returns>
    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync([FromBody] TokenRequest inTokenRequest)
    {
        ApiResult<TokenResponse> apiResult = null;
        try
        {
            // Attempt to create a token based on the provided request
            apiResult = await this._tokenService.CreateToken(inTokenRequest);
        }
        catch (Exception ex)
        {
            // Handle exceptions and log them
            apiResult = ApiResult<TokenResponse>.FailedResult(ex.Message);
            this._systemLogger.LogExceptionMessage(ELoggingLevel.Error, $"{nameof(TokenResponse)}/RequestToken failed!", ex);
        }

        return this.Ok(apiResult);
    }

    /// <summary>
    /// Registers a new user account.
    /// </summary>
    /// <param name="registerDto">The user registration data.</param>
    /// <returns>An IActionResult containing the registered user or an error message.</returns>
    [AllowAnonymous]
    [HttpPost("Register")]
    [DtoModelValidationFilter]
    public async Task<ActionResult<UserDto>> RegisterAsync([FromBody] UserRegisterDto registerDto)
    {
        ApiResult<UserDto> apiResult = null;
        try
        {
            // Attempt to register a new user account
            apiResult = await this._accountService.RegisterUser(registerDto);
        }
        catch (Exception ex)
        {
            // Handle exceptions and log them
            apiResult = ApiResult<UserDto>.FailedResult(ex.Message);
            this._systemLogger.LogExceptionMessage(ELoggingLevel.Error, $"{nameof(UserDto)}/Register failed!", ex);
        }

        return this.Ok(apiResult);
    }

    /// <summary>
    /// Checks if an email address already exists in the system.
    /// </summary>
    /// <param name="email">The email address to check.</param>
    /// <returns>An IActionResult indicating email existence or an error message.</returns>
    [AllowAnonymous]
    [HttpGet("ExistsEmail")]
    public async Task<ActionResult<UserDto>> CheckEmailAsync([FromQuery] string email)
    {
        ApiResult<UserDto> apiResult = null;
        try
        {
            // Attempt to find a user by email
            apiResult = await this._accountService.FindByEmail(email);
        }
        catch (Exception ex)
        {
            // Handle exceptions and log them
            apiResult = ApiResult<UserDto>.FailedResult(ex.Message);
            this._systemLogger.LogExceptionMessage(ELoggingLevel.Error, $"{nameof(UserDto)}/CheckEmailAsync failed!", ex);
        }

        return this.Ok(apiResult);
    }

    /// <summary>
    /// Retrieves the currently authenticated user's information.
    /// </summary>
    /// <returns>An IActionResult containing the current user's information or an error message.</returns>
    [Authorize]
    [HttpGet("CurrentUser")]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        ApiResult<UserDto> apiResult = null;
        try
        {
            // Attempt to get the current user's information
            apiResult = await this._accountService.GetCurrentUser(this.HttpContext);
        }
        catch (Exception ex)
        {
            // Handle exceptions and log them
            apiResult = ApiResult<UserDto>.FailedResult(ex.Message);
            this._systemLogger.LogExceptionMessage(ELoggingLevel.Error, $"{nameof(UserDto)}/GetCurrentUser failed!", ex);
        }

        return this.Ok(apiResult);
    }
}