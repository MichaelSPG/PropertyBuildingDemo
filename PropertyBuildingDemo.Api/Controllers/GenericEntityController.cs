// <copyright file="GenericEntityController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PropertyBuildingDemo.Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities.Enums;
using PropertyBuildingDemo.Domain.Interfaces;

/// <summary>
/// Base controller for handling CRUD operations for generic entities of type TEntity.
/// </summary>
/// <typeparam name="TEntity">The entity type this controller operates on.</typeparam>
[Authorize(AuthenticationSchemes = "JwtClient")]
public class GenericEntityController<TEntity> : BaseController
    where TEntity : BaseEntityDb
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemLogger _systemLogger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericEntityController{TEntity}"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work for database operations.</param>
    /// <param name="systemLogger">The system logger for logging exceptions.</param>
    public GenericEntityController(IUnitOfWork unitOfWork, ISystemLogger systemLogger)
    {
        this._unitOfWork = unitOfWork;
        this._systemLogger = systemLogger;
    }

    /// <summary>
    /// Retrieves a list of entities of type TEntity.
    /// </summary>
    /// <returns>An IActionResult containing the list of entities or an error message.</returns>
    [HttpGet("List")]
    public async Task<IActionResult> Index()
    {
        ApiResult<IEnumerable<TEntity>> apiResult = null;
        try
        {
            // Retrieve all entities of type TEntity from the database
            apiResult = ApiResult<IEnumerable<TEntity>>.SuccessResult(
                await this._unitOfWork.GetRepository<TEntity>().Entities.ToListAsync());
        }
        catch (Exception ex)
        {
            // Handle exceptions and log them
            apiResult = ApiResult<IEnumerable<TEntity>>.FailedResult(ex.Message);
            this._systemLogger.LogExceptionMessage(ELoggingLevel.Error, $"{nameof(TEntity)}/Index failed!", ex);
        }

        return this.Ok(apiResult);
    }

    /// <summary>
    /// Retrieves an entity of type TEntity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <returns>An IActionResult containing the entity or an error message if not found.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        ApiResult<TEntity> apiResult = null;
        try
        {
            // Retrieve the entity by its ID using the unit of work and repository
            var result = await this._unitOfWork.GetRepository<TEntity>().GetAsync(id);

            // Check if the entity was found or not, and create an appropriate API result
            apiResult = result == null ? ApiResult<TEntity>.FailedResult() : ApiResult<TEntity>.SuccessResult(result);
        }
        catch (Exception ex)
        {
            // Handle exceptions and log them
            apiResult = ApiResult<TEntity>.FailedResult(ex.Message);
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
    public async Task<IActionResult> Insert([FromBody] TEntity inTEntity)
    {
        ApiResult<TEntity> apiResult = null;
        try
        {
            // Add the new entity to the repository
            var result = await this._unitOfWork.GetRepository<TEntity>().AddAsync(inTEntity);

            // Complete the unit of work transaction and check if the operation succeeded
            bool succeeded = await this._unitOfWork.Complete();

            // Create an appropriate API result based on the operation's success
            apiResult = result == null ? ApiResult<TEntity>.FailedResult($"Failed to insert {nameof(TEntity)}") : ApiResult<TEntity>.SuccessResult(result);
        }
        catch (Exception ex)
        {
            // Handle exceptions and log them
            apiResult = ApiResult<TEntity>.FailedResult(ex.Message);
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
    public async Task<IActionResult> Update([FromBody] TEntity inTEntity)
    {
        ApiResult<TEntity> apiResult = null;
        try
        {
            // Update the entity in the repository
            var result = await this._unitOfWork.GetRepository<TEntity>().UpdateAsync(inTEntity);

            // Complete the unit of work transaction and check if the operation succeeded
            bool succeeded = await this._unitOfWork.Complete();

            // Create an appropriate API result based on the operation's success
            apiResult = result == null ? ApiResult<TEntity>.FailedResult($"Failed to Update {nameof(TEntity)}") : ApiResult<TEntity>.SuccessResult(result);
        }
        catch (Exception ex)
        {
            // Handle exceptions and log them
            apiResult = ApiResult<TEntity>.FailedResult(ex.Message);
            this._systemLogger.LogExceptionMessage(ELoggingLevel.Error, $"{nameof(TEntity)}/Update failed!", ex);
        }

        return this.Ok(apiResult);
    }

    /// <summary>
    /// Deletes an entity of type TEntity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    /// <returns>An IActionResult indicating success or failure.</returns>
    [HttpDelete("Delete")]

    public async Task<IActionResult> Delete([FromBody] long id)
    {
        ApiResult<TEntity> apiResult = null;
        try
        {
            // Retrieve the entity by its ID
            var item = await this._unitOfWork.GetRepository<TEntity>().GetAsync(id);
            if (item != null)
            {
                // Delete the entity and complete the unit of work transaction
                await this._unitOfWork.GetRepository<TEntity>().DeleteAsync(item);
                bool succeeded = await this._unitOfWork.Complete();
                apiResult = !succeeded ? ApiResult<TEntity>.FailedResult($"Failed to Delete {nameof(TEntity)}") : ApiResult<TEntity>.SuccessResult();
            }
            else
            {
                apiResult = ApiResult<TEntity>.FailedResult($"Failed to Delete {nameof(TEntity)}");
            }
        }
        catch (Exception ex)
        {
            apiResult = ApiResult<TEntity>.FailedResult(ex.Message);
            this._systemLogger.LogExceptionMessage(ELoggingLevel.Error, $"{nameof(TEntity)}/Update failed!", ex);
        }

        return this.Ok(apiResult);
    }
}