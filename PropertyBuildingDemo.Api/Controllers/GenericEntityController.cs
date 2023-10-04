// <copyright file="GenericEntityController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoMapper;
using PropertyBuildingDemo.Application.IServices;

namespace PropertyBuildingDemo.Api.Controllers;
using PropertyBuildingDemo.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities.Enums;
using PropertyBuildingDemo.Domain.Interfaces;

/// <summary>
/// Base controller for handling CRUD operations for generic entities of type TEntity.
/// </summary>
/// <typeparam name="TEntity">The entity type this controller operates on.</typeparam>
/// <typeparam name="TEntityDto">The DTO entity to map out</typeparam>
[Authorize(AuthenticationSchemes = "JwtClient")]
public class GenericEntityController<TEntity, TEntityDto> : BaseController
    where TEntity : BaseEntityDb
    where TEntityDto : class
{
    private readonly ISystemLogger _systemLogger;
    private readonly IDbEntityServices<TEntity, TEntityDto> _dbEntityServices;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericEntityController{TEntity}"/> class.
    /// </summary>
    /// <param name="systemLogger">The system logger for logging exceptions.</param>
    /// <param name="dbEntityServices">The entity services</param>
    public GenericEntityController(ISystemLogger systemLogger, IDbEntityServices<TEntity, TEntityDto> dbEntityServices)
    {
        this._systemLogger = systemLogger;
        this._dbEntityServices = dbEntityServices;
    }

    /// <summary>
    /// Gets a list of entities with optional filtering for deleted entities.
    /// </summary>
    /// <returns>A list of entities.</returns>
    [HttpGet("List")]
    public async Task<IActionResult> Index()
    {
        ApiResult<IEnumerable<TEntityDto>> apiResult = null;
        try
        {
            var result = await this._dbEntityServices.GetAllAsync();
            apiResult = await ApiResult<IEnumerable<TEntityDto>>.SuccessResultAsync(result);
        }
        catch (Exception ex)
        {
            // Handle exceptions and log them
            apiResult = await ApiResult<IEnumerable<TEntityDto>>.FailedResultAsync(ex.Message);
            this._systemLogger.LogExceptionMessage(ELoggingLevel.Error, $"{nameof(TEntity)}/Index failed!", ex);
        }

        return this.Ok(apiResult);
    }

    /// <summary>
    /// Retrieves an entity of type TEntity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <returns>An IActionResult containing the entity or an error message if not found.</returns>
    [HttpGet("ById/{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        ApiResult<TEntityDto> apiResult = null;
        try
        {
            var result = await this._dbEntityServices.GetByIdAsync(id);
            // Check if the entity was found or not, and create an appropriate API result
            apiResult = result == null ? await ApiResult<TEntityDto>.SuccessResultAsync("not found") :
                await ApiResult<TEntityDto>.SuccessResultAsync(result);
        }
        catch (Exception ex)
        {
            // Handle exceptions and log them
            apiResult = await ApiResult<TEntityDto>.FailedResultAsync(ex.Message);
            this._systemLogger.LogExceptionMessage(ELoggingLevel.Error, $"{nameof(TEntity)}/GetById failed!", ex);
        }

        return this.Ok(apiResult);
    }

    /// <summary>
    /// Inserts a new entity of type TEntity.
    /// </summary>
    /// <param name="inTEntity">The entity to insert.</param>
    /// <returns>An IActionResult containing the inserted entity or an error message.</returns>
    [HttpPost("Insert")]
    public async Task<IActionResult> Insert([FromBody] TEntityDto inTEntity)
    {
        ApiResult<TEntityDto> apiResult = null;
        try
        {
            var result = await this._dbEntityServices.AddAsync(inTEntity);

            // Create an appropriate API result based on the operation's success
            apiResult = result == null ? await ApiResult<TEntityDto>.FailedResultAsync($"Failed to insert {nameof(TEntityDto)}") :
                await ApiResult<TEntityDto>.SuccessResultAsync(result);
        }
        catch (Exception ex)
        {
            // Handle exceptions and log them
            apiResult = await ApiResult<TEntityDto>.FailedResultAsync(ex.Message);
            this._systemLogger.LogExceptionMessage(ELoggingLevel.Error, $"{nameof(TEntity)}/Insert failed!", ex);
        }

        return this.Ok(apiResult);
    }

    /// <summary>
    /// Updates an existing entity of type TEntity.
    /// </summary>
    /// <param name="inTEntity">The entity with updated data.</param>
    /// <returns>An IActionResult containing the updated entity or an error message.</returns>
    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] TEntityDto inTEntity)
    {
        ApiResult<TEntityDto> apiResult = null;
        try
        {
            // Update the entity in the repository
            var result = await this._dbEntityServices.UpdateAsync(inTEntity);

            // Create an appropriate API result based on the operation's success
            apiResult = result == null ? await ApiResult<TEntityDto>.FailedResultAsync($"Failed to Update {nameof(TEntity)}") :
                await ApiResult<TEntityDto>.SuccessResultAsync(result);
        }
        catch (Exception ex)
        {
            // Handle exceptions and log them
            apiResult = await ApiResult<TEntityDto>.FailedResultAsync(ex.Message);
            this._systemLogger.LogExceptionMessage(ELoggingLevel.Error, $"{nameof(TEntity)}/Update failed!", ex);
        }

        return this.Ok(apiResult);
    }

    /// <summary>
    /// Deletes an entity of type TEntity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    /// <returns>An IActionResult indicating success or failure.</returns>
    [HttpDelete("Delete/{id}")]

    public async Task<IActionResult> Delete(long id)
    {
        ApiResult<TEntityDto> apiResult = null;
        try
        {
            // Retrieve the entity by its ID
            var item = await this._dbEntityServices.GetByIdAsync(id);
            if (item != null)
            {
                // Delete the entity and complete the unit of work transaction
                item = await this._dbEntityServices.DeleteAsync(item);

                apiResult = item == null ? await ApiResult<TEntityDto>.FailedResultAsync($"Failed to Delete {nameof(TEntityDto)}") :
                    await ApiResult<TEntityDto>.SuccessResultAsync(item);
            }
            else
            {
                apiResult = await ApiResult<TEntityDto>.FailedResultAsync($"Failed to Delete {nameof(TEntity)} because it does not exist");
            }
        }
        catch (Exception ex)
        {
            apiResult = await ApiResult<TEntityDto>.FailedResultAsync(ex.Message + ex.StackTrace.ToString());
            this._systemLogger.LogExceptionMessage(ELoggingLevel.Error, $"{nameof(TEntity)}/Update failed!", ex);
        }

        return this.Ok(apiResult);
    }
}