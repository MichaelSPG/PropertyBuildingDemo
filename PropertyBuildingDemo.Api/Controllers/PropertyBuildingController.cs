using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Application.Services;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Domain.Entities.Enums;
using PropertyBuildingDemo.Domain.Interfaces;

namespace PropertyBuildingDemo.Api.Controllers
{
    /// <summary>
    /// The main requested API controller
    /// </summary>
    [Authorize(AuthenticationSchemes = "JwtClient")]
    public class PropertyBuildingController : BaseController
    {
        private readonly IPropertyBuildingService _propertyBuildingService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISystemLogger _systemLogger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="propertyBuildingService">dependency Injection</param>
        public PropertyBuildingController(IPropertyBuildingService propertyBuildingService, IUnitOfWork inUnitOfWork, ISystemLogger systemLogger)
        {
            _propertyBuildingService = propertyBuildingService;
            _unitOfWork = inUnitOfWork;
            _systemLogger = systemLogger;
        }

        [HttpPost("ListBy")]
        public async Task<IActionResult> GetListBy([FromBody] DefaultQueryFilterArgs inFilterArgs)
        {
            ApiResult<IEnumerable<Property>> apiResult = null;
            try
            {
                apiResult = ApiResult<IEnumerable<Property>>.SuccessResult(await _propertyBuildingService.FilterPropertyBuildings(inFilterArgs));
            }
            catch (Exception ex)
            {
                apiResult = ApiResult<IEnumerable<Property>>.FailedResult(ex.Message);
                _systemLogger.LogExceptionMessage(ELogginLevel.Level_Error, $"{nameof(IEnumerable<Property>)}/GetListBy failed!", ex);
            }

            return Ok(apiResult);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreatePropertyBuilding([FromBody] PropertyDto inPropertyDto)
        {
            ApiResult<Property> apiResult = null;
            try
            {
                apiResult = ApiResult<Property>.SuccessResult(await _propertyBuildingService.CreatePropertyBuilding(inPropertyDto));
            }
            catch (Exception ex)
            {
                apiResult = ApiResult<Property>.FailedResult(ex.Message);
                _systemLogger.LogExceptionMessage(ELogginLevel.Level_Error, $"{nameof(Property)}/CreatePropertyBuilding failed!", ex);
            }

            return Ok(apiResult);
        }

        [HttpPost("AddImageFromProperty")]
        public async Task<IActionResult> AddImageFromProperty([FromBody] PropertyImageDto inPropertyImageDto)
        {
            ApiResult<PropertyImage> apiResult = null;
            try
            {
                apiResult = ApiResult<PropertyImage>.SuccessResult(await _propertyBuildingService.AddImageFromProperty(inPropertyImageDto));
            }
            catch (Exception ex)
            {
                apiResult = ApiResult<PropertyImage>.FailedResult(ex.Message);
                _systemLogger.LogExceptionMessage(ELogginLevel.Level_Error, $"{nameof(PropertyImage)}/AddImageFromProperty failed!", ex);
            }

            return Ok(apiResult);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateProperty([FromBody] PropertyDto inPropertyDto)
        {
            ApiResult<Property> apiResult = null;
            try
            {
                apiResult = ApiResult<Property>.SuccessResult(await _propertyBuildingService.UpdatePropertyBuilding(inPropertyDto));
            }
            catch (Exception ex)
            {
                apiResult = ApiResult<Property>.FailedResult(ex.Message);
                _systemLogger.LogExceptionMessage(ELogginLevel.Level_Error, $"{nameof(Property)}/UpdateProperty failed!", ex);
            }

            return Ok(apiResult);
        }

        [HttpPut("ChangePrice")]
        public async Task<IActionResult> AddImageFromProperty([FromQuery] long inIdProperty, [FromQuery] decimal inNewPrice)
        {
            ApiResult<Property> apiResult = null;
            try
            {
                apiResult = ApiResult<Property>.SuccessResult(await _propertyBuildingService.ChangePrice(inIdProperty, inNewPrice));
            }
            catch (Exception ex)
            {
                apiResult = ApiResult<Property>.FailedResult(ex.Message);
                _systemLogger.LogExceptionMessage(ELogginLevel.Level_Error, $"{nameof(Property)}/AddImageFromProperty failed!", ex);
            }

            return Ok(apiResult);
        }

    }
}
