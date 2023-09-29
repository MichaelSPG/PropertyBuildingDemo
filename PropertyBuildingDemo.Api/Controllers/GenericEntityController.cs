using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyBuildingDemo.Api.Controllers;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities.Enums;
using PropertyBuildingDemo.Domain.Interfaces;

namespace TEntityBuildingDemo.Api.Controllers
{
    public class GenericEntityController <TEntity> : BaseController where TEntity : BaseEntityDB
    {
        readonly IUnitOfWork _unitOfWork;
        private readonly ISystemLogger _systemLogger;

        public GenericEntityController(IUnitOfWork unitOfWork, ISystemLogger systemLogger)
        {
            _unitOfWork = unitOfWork;
            _systemLogger = systemLogger;
        }
        [HttpGet("list")]
        public async Task<IActionResult> Index()
        {
            ApiResult<IEnumerable< TEntity>> apiResult = null;
            try
            {
                apiResult = ApiResult<IEnumerable<TEntity>>.SuccessResult(
                    await _unitOfWork.GetRepository<TEntity>().Entities.ToListAsync()
                );                
            }
            catch (Exception ex)
            {
                apiResult = ApiResult< IEnumerable<TEntity>>.FailedResult(ex.Message);
                _systemLogger.LogExceptionMessage(ELogginLevel.Level_Error, $"{nameof(TEntity)}/Index failed!", ex);
            }

            return Ok(apiResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            ApiResult<TEntity> apiResult = null;
            try
            {
                var result = await _unitOfWork.GetRepository<TEntity>().GetAsync(id);
                apiResult = result == null ? ApiResult<TEntity>.FailedResult() : ApiResult<TEntity>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                apiResult = ApiResult<TEntity>.FailedResult(ex.Message);
                _systemLogger.LogExceptionMessage(ELogginLevel.Level_Error, $"{nameof(TEntity)}/GetById failed!", ex);
            }

            return Ok(apiResult);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromBody] TEntity InTEntity)
        {
            ApiResult<TEntity> apiResult = null;
            try
            {
                var result = await _unitOfWork.GetRepository<TEntity>().AddAsync(InTEntity);
                bool succeeded = await _unitOfWork.Complete();
                apiResult = result == null ? ApiResult<TEntity>.FailedResult($"Failed to insert {nameof(TEntity)}") : ApiResult<TEntity>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                apiResult = ApiResult<TEntity>.FailedResult(ex.Message);
                _systemLogger.LogExceptionMessage(ELogginLevel.Level_Error, $"{nameof(TEntity)}/Insert failed!", ex);
            }

            return Ok(apiResult);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] TEntity InTEntity)
        {
            ApiResult<TEntity> apiResult = null;
            try
            {
                var result = await _unitOfWork.GetRepository<TEntity>().UpdateAsync(InTEntity);
                bool succeeded = await _unitOfWork.Complete();
                apiResult = result == null ? ApiResult<TEntity>.FailedResult($"Failed to Update {nameof(TEntity)}") : ApiResult<TEntity>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                apiResult = ApiResult<TEntity>.FailedResult(ex.Message);
                _systemLogger.LogExceptionMessage(ELogginLevel.Level_Error, $"{nameof(TEntity)}/Update failed!", ex);
            }

            return Ok(apiResult);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]

        public async Task<IActionResult> Delete([FromBody] long id)
        {
            ApiResult<TEntity> apiResult = null;
            try
            {
                var item = await _unitOfWork.GetRepository<TEntity>().GetAsync(id);
                if(item != null)
                {
                    await _unitOfWork.GetRepository<TEntity>().DeleteAsync(item);
                    bool succeeded = await _unitOfWork.Complete();
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
                _systemLogger.LogExceptionMessage(ELogginLevel.Level_Error, $"{nameof(TEntity)}/Update failed!", ex);
            }

            return Ok(apiResult);
        }
    }
}
