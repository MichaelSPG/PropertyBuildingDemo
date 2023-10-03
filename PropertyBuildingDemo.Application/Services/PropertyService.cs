using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Application.Helpers;
using PropertyBuildingDemo.Application.IServices;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Domain.Entities.Enums;
using PropertyBuildingDemo.Domain.Specifications;
using System.Linq.Expressions;

namespace PropertyBuildingDemo.Application.Services
{
    /// <summary>
    /// Service for managing properties.
    /// </summary>
    public class PropertyService : IPropertyBuildingService
    {
        private readonly IDbEntityServices<Owner, OwnerDto> _ownerEntityServices;
        private readonly IDbEntityServices<Property, PropertyDto> _entityServices;
        private readonly IDbEntityServices<PropertyImage, PropertyImageDto> _propertyImageService;
        private readonly IDbEntityServices<PropertyImage, PropertyImageDto> _propertyTraceService;
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work used for data access.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        /// <param name="inImageService">The property image service.</param>
        /// <param name="inPropertyTraceService">The property trace service.</param>
        public PropertyService(IDbEntityServices<Property, PropertyDto> entityServices, IDbEntityServices<PropertyImage, PropertyImageDto> propertyImageService, IDbEntityServices<PropertyImage, PropertyImageDto> propertyTraceService, IDbEntityServices<Owner, OwnerDto> ownerEntityServices)
        {
            _entityServices = entityServices;
            _propertyImageService = propertyImageService;
            _propertyTraceService = propertyTraceService;
            _ownerEntityServices = ownerEntityServices;
        }

        /// <summary>
        /// Creates a new property building.
        /// </summary>
        /// <param name="inPropertyDto">The property DTO.</param>
        /// <returns>The created property.</returns>
        public async Task<PropertyDto> CreatePropertyBuilding(PropertyDto inPropertyDto)
        {
            var owner = await _ownerEntityServices.GetByIdAsync(inPropertyDto.IdOwner);

            if (owner == null)
            {
                throw new ArgumentException("IdOwner does not exist!");
            }

            return await _entityServices.AddAsync(inPropertyDto);
        }
        /// <summary>
        /// Adds an image to a property.
        /// </summary>
        /// <param name="inImageDto">The property image DTO containing image information.</param>
        /// <returns>The added property DTO with the new image information.</returns>
        /// <exception cref="ArgumentException">Thrown when the specified property identifier (IdProperty) does not exist.</exception>
        public async Task<PropertyDto> AddImageFromProperty(PropertyImageDto inImageDto)
        {
            PropertyDto property = await _entityServices.GetByIdAsync(inImageDto.IdProperty);

            if (property == null)
            {
                throw new ArgumentException("IdProperty does not exist!");
            }

            await _propertyImageService.AddAsync(inImageDto);

            return await _entityServices.GetByIdAsync(inImageDto.IdProperty);
        }

        /// <summary>
        /// Changes the price of a property.
        /// </summary>
        /// <param name="inPropertyId">The ID of the property.</param>
        /// <param name="inNewPrice">The new price.</param>
        /// <returns>The updated property.</returns>
        public async Task<PropertyDto> ChangePrice(long inPropertyId, decimal inNewPrice)
        {
            PropertyDto property = await _entityServices.GetByIdAsync(inPropertyId);
            if (property == null)
            {
                throw new ArgumentException("IdProperty does not exist!");
            }
            if (property.Price == inNewPrice)
            {
                return property;
            };
            property.Price = inNewPrice;
            return await _entityServices.UpdateAsync(property);
        }

        /// <summary>
        /// Updates a property building.
        /// </summary>
        /// <param name="inPropertyDto">The updated property DTO.</param>
        /// <returns>The updated property.</returns>
        public async Task<PropertyDto> UpdatePropertyBuilding(PropertyDto inPropertyDto)
        {
            var currentValue = await _entityServices.GetByIdAsync(inPropertyDto.IdProperty);
            if (currentValue == null || inPropertyDto.IdProperty < 0)
            {
                throw new ArgumentException("IdProperty was not found!");
            }
            if (string.IsNullOrWhiteSpace(inPropertyDto.InternalCode))
            {
                inPropertyDto.InternalCode = currentValue.InternalCode;
            }

            return await _entityServices.UpdateAsync(inPropertyDto);
        }

        

        /// <summary>
        /// Filters property buildings based on query filter arguments.
        /// </summary>
        /// <param name="inFilterArgs">The query filter arguments.</param>
        /// <returns>The filtered properties.</returns>
        public async Task<IEnumerable<PropertyDto>> FilterPropertyBuildings(DefaultQueryFilterArgs inFilterArgs)
        {
            IEnumerable<Property> properties;
            if (inFilterArgs == null)
                return await _entityServices.GetAllAsync();

            if (inFilterArgs.SortingParameters == null && !inFilterArgs.SortingParameters.Any())
            {
                inFilterArgs.SortingParameters.ToList().Add(new SortingParameters
                {
                    Direction = ESortingDirection.Ascending,
                    TargetField = "IdProperty",
                    Priority = 0
                });
            }
            var includes = new List<Expression<Func<Property, object>>>
            {
                x => x.PropertyImages,
                x => x.PropertyTraces,
            };
            
            PropertySpecification propertySpecification = new PropertySpecification(ValidationExpressions.GetSpecificationsFromFilters<Property>(inFilterArgs), includes);

            if (inFilterArgs.PageSize > 0)
            {
                propertySpecification.ApplyingPaging(inFilterArgs.PageSize, inFilterArgs.PageIndex * inFilterArgs.PageSize);
            }

            return await _entityServices.GetByAsync(propertySpecification);  
        }

        /// <summary>
        /// Retrieves detailed information about a property or building based on its unique identifier.
        /// </summary>
        /// <param name="inId">The unique identifier of the property or building to retrieve.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// The task result is a <see cref="PropertyDto"/> containing detailed information about the retrieved property or building.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the specified property identifier (IdProperty) does not exist.</exception>
        public async Task<PropertyDto> GetPropertyBuildingById(long inId)
        {
            PropertyDto property = await _entityServices.GetByIdAsync(inId);

            if (property == null)
            {
                throw new ArgumentException("IdProperty does not exist!");
            }

            // Return the property DTO
            return property;
        }
    }
}
