// <copyright file="PropertyBuildingController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using PropertyBuildingDemo.Application.IServices;

namespace PropertyBuildingDemo.Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities.Enums;
using PropertyBuildingDemo.Domain.Interfaces;

/// <summary>
/// Controller responsible for managing property building-related operations.
/// </summary>
[Authorize(AuthenticationSchemes = "JwtClient")]
public class PropertyBuildingController : BaseController
{
    private readonly IPropertyBuildingService _propertyBuildingService; // Service for property building operations
    private readonly IUnitOfWork _unitOfWork; // Unit of work for database operations
    private readonly ISystemLogger _systemLogger; // Logger for logging exceptions

    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyBuildingController"/> class.
    /// </summary>
    /// <param name="propertyBuildingService">The service for property building operations.</param>
    /// <param name="unitOfWork">The unit of work for database operations.</param>
    /// <param name="systemLogger">The system logger for logging exceptions.</param>
    public PropertyBuildingController(IPropertyBuildingService propertyBuildingService, IUnitOfWork unitOfWork, ISystemLogger systemLogger)
    {
        this._propertyBuildingService = propertyBuildingService;
        this._unitOfWork = unitOfWork;
        this._systemLogger = systemLogger;
    }

    /// <summary>
    /// Retrieves a list of properties based on the provided filter.
    /// </summary>
    /// <param name="inFilterArgs">The filter criteria.</param>
    /// <returns>An IActionResult containing the list of properties or an error message.</returns>
    [HttpGet("ListBy")]
    public async Task<IActionResult> GetListBy([FromQuery] DefaultQueryFilterArgs inFilterArgs)
    {
        ApiResult<IEnumerable<PropertyDto>> apiResult = null;
        try
        {
            // Attempt to filter and retrieve a list of properties
            apiResult = ApiResult<IEnumerable<PropertyDto>>.SuccessResult(await this._propertyBuildingService.FilterPropertyBuildings(inFilterArgs));
        }
        catch (Exception ex)
        {
            string title = $"{nameof(PropertyDto)}/GetListBy failed!->{ex.Message}";
            apiResult = ApiResult<IEnumerable<PropertyDto>>.FailedResult(title);
            this._systemLogger.LogExceptionMessage(ELoggingLevel.Error, $"{nameof(PropertyBuildingController)}/GetListBy failed!", ex);
        }

        return this.Ok(apiResult);
    }

    /// <summary>
    /// Retrieves a property by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the property to retrieve.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> representing the HTTP response.
    /// If successful, it returns the property information in the response body.
    /// If an error occurs, it returns an error message in the response body.
    /// </returns>
    [HttpGet("ById")]
    public async Task<IActionResult> GetById([FromQuery] long id)
    {
        ApiResult<PropertyDto> apiResult = null;
        try
        {
            // Attempt to filter and retrieve a property by its unique identifier
            apiResult = ApiResult<PropertyDto>.SuccessResult(await this._propertyBuildingService.GetPropertyBuildingById(id));
        }
        catch (Exception ex)
        {
            string title = $"{nameof(PropertyDto)}/GetListBy failed!->{ex.Message}";
            apiResult = ApiResult<PropertyDto>.FailedResult(title);
            this._systemLogger.LogExceptionMessage(ELoggingLevel.Error, $"{nameof(PropertyBuildingController)}/GetListBy failed!", ex);
        }

        return this.Ok(apiResult);
    }

    /// <summary>
    /// Creates a new property building.
    /// </summary>
    /// <param name="inPropertyDto">The property details for creation.</param>
    /// <returns>An IActionResult indicating success or an error message.</returns>
    [HttpPost("Create")]
    public async Task<IActionResult> CreatePropertyBuilding([FromBody] PropertyDto inPropertyDto)
    {
        ApiResult<PropertyDto> apiResult = null;
        try
        {
            // Attempt to create a new property building
            apiResult = ApiResult<PropertyDto>.SuccessResult(await this._propertyBuildingService.CreatePropertyBuilding(inPropertyDto));
        }
        catch (Exception ex)
        {
            apiResult = ApiResult<PropertyDto>.FailedResult(ex.Message);
            this._systemLogger.LogExceptionMessage(ELoggingLevel.Error, $"{nameof(PropertyBuildingController)}/CreatePropertyBuilding failed!", ex);
        }

        return this.Ok(apiResult);
    }

    /// <summary>
    /// Adds an image to a property.
    /// </summary>
    /// <param name="inPropertyImageDto">The image details to add to the property.</param>
    /// <returns>An IActionResult indicating success or an error message.</returns>
    [HttpPost("AddImageFromProperty")]
    public async Task<IActionResult> AddImageFromProperty([FromBody] PropertyImageDto inPropertyImageDto)
    {
        ApiResult<PropertyDto> apiResult = null;
        try
        {
            // Attempt to add an image to a property
            apiResult = ApiResult<PropertyDto>.SuccessResult(await this._propertyBuildingService.AddImageFromProperty(inPropertyImageDto));
        }
        catch (Exception ex)
        {
            apiResult = ApiResult<PropertyDto>.FailedResult(ex.Message);
            this._systemLogger.LogExceptionMessage(ELoggingLevel.Error, $"{nameof(PropertyBuildingController)}/AddImageFromProperty failed!", ex);
        }

        return this.Ok(apiResult);
    }

    /// <summary>
    /// Updates an existing property.
    /// </summary>
    /// <param name="inPropertyDto">The updated property details.</param>
    /// <returns>An IActionResult indicating success or an error message.</returns>
    [HttpPut("Update")]
    public async Task<IActionResult> UpdateProperty([FromBody] PropertyDto inPropertyDto)
    {
        ApiResult<PropertyDto> apiResult = null;
        try
        {
            // Attempt to update an existing property
            apiResult = ApiResult<PropertyDto>.SuccessResult(await this._propertyBuildingService.UpdatePropertyBuilding(inPropertyDto));
        }
        catch (Exception ex)
        {
            apiResult = ApiResult<PropertyDto>.FailedResult(ex.Message);
            this._systemLogger.LogExceptionMessage(ELoggingLevel.Error, $"{nameof(PropertyBuildingController)}/UpdateProperty failed!", ex);
        }

        return this.Ok(apiResult);
    }

    /// <summary>
    /// Changes the price of a property.
    /// </summary>
    /// <param name="inIdProperty">The ID of the property to update.</param>
    /// <param name="inNewPrice">The new price of the property.</param>
    /// <returns>An IActionResult indicating success or an error message.</returns>
    [HttpPut("ChangePrice")]
    public async Task<IActionResult> ChangePrice([FromQuery] long inIdProperty, [FromQuery] decimal inNewPrice)
    {
        ApiResult<PropertyDto> apiResult = null;
        try
        {
            // Attempt to change the price of the property
            apiResult = ApiResult<PropertyDto>.SuccessResult(await this._propertyBuildingService.ChangePrice(inIdProperty, inNewPrice));
        }
        catch (Exception ex)
        {
            apiResult = ApiResult<PropertyDto>.FailedResult(ex.Message);
            this._systemLogger.LogExceptionMessage(ELoggingLevel.Error, $"{nameof(PropertyBuildingController)}/ChangePrice failed!", ex);
        }

        return this.Ok(apiResult);
    }
}