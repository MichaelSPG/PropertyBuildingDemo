using AutoMapper;
using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Application.Helpers;
using PropertyBuildingDemo.Application.IServices;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Domain.Entities.Enums;
using PropertyBuildingDemo.Domain.Interfaces;
using PropertyBuildingDemo.Domain.Specifications;

namespace PropertyBuildingDemo.Application.Services
{
    /// <summary>
    /// Service for managing properties.
    /// </summary>
    public class PropertyService : IPropertyBuildingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPropertyImageService _inImageService;
        private readonly IPropertyTraceService _inPropertyTraceService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work used for data access.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        /// <param name="inImageService">The property image service.</param>
        /// <param name="inPropertyTraceService">The property trace service.</param>
        public PropertyService(IUnitOfWork unitOfWork, IMapper mapper, IPropertyImageService inImageService, IPropertyTraceService inPropertyTraceService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _inImageService = inImageService;
            _inPropertyTraceService = inPropertyTraceService;
        }

        /// <summary>
        /// Creates a new property building.
        /// </summary>
        /// <param name="inPropertyDto">The property DTO.</param>
        /// <returns>The created property.</returns>
        public async Task<PropertyDto> CreatePropertyBuilding(PropertyDto inPropertyDto)
        {
            Owner owner = await _unitOfWork.GetRepository<Owner>().GetAsync(inPropertyDto.IdOwner);

            if (owner == null)
            {
                throw new ArgumentException("IdOwner does not exist!");
            }

            await _unitOfWork.BeginTransaction();

            Property newProperty = _mapper.Map<Property>(inPropertyDto);
            newProperty = await _unitOfWork.GetRepository<Property>().AddAsync(newProperty);
            await _unitOfWork.Complete(inFinishTransaction: false);
            // Add images if any
            if (inPropertyDto.PropertyImages.Any())
            {
                foreach (var propertyImageDto in inPropertyDto.PropertyImages)
                {
                    propertyImageDto.IdProperty = newProperty.IdProperty;
                }
                var propertyImages = _mapper.Map<IEnumerable<PropertyImage>>(inPropertyDto.PropertyImages);
                propertyImages = await _inImageService.AddMultipleImages(propertyImages);
            }

            // Add traces if any
            if (inPropertyDto.PropertyTraces.Any())
            {
                foreach (var propertyTraceDto in inPropertyDto.PropertyTraces)
                {
                    propertyTraceDto.IdProperty = newProperty.IdProperty;
                }
                var propertyTraces = _mapper.Map<IEnumerable<PropertyTrace>>(inPropertyDto.PropertyTraces);
                propertyTraces = await _inPropertyTraceService.AddMultipleTraces(propertyTraces);
            }

            await _unitOfWork.Complete();
            
            return _mapper.Map<PropertyDto>(newProperty);
        }
        /// <summary>
        /// Adds an image to a property.
        /// </summary>
        /// <param name="inImageDto">The property image DTO containing image information.</param>
        /// <returns>The added property DTO with the new image information.</returns>
        /// <exception cref="ArgumentException">Thrown when the specified property identifier (IdProperty) does not exist.</exception>
        public async Task<PropertyDto> AddImageFromProperty(PropertyImageDto inImageDto)
        {
            PropertyImage newPropertyImage = _mapper.Map<PropertyImage>(inImageDto);

            Property property = await _unitOfWork.GetRepository<Property>().GetAsync(newPropertyImage.IdProperty);

            if (property == null)
            {
                throw new ArgumentException("IdProperty does not exist!");
            }

            await _unitOfWork.GetRepository<PropertyImage>().AddAsync(newPropertyImage);
            await _unitOfWork.Complete();

            property = await _unitOfWork.GetRepository<Property>().GetAsync(property.IdProperty);
            // Return the added property DTO with the new image information.
            return _mapper.Map<PropertyDto>(property);
        }

        /// <summary>
        /// Changes the price of a property.
        /// </summary>
        /// <param name="inPropertyId">The ID of the property.</param>
        /// <param name="inNewPrice">The new price.</param>
        /// <returns>The updated property.</returns>
        public async Task<PropertyDto> ChangePrice(long inPropertyId, decimal inNewPrice)
        {
            Property property = await _unitOfWork.GetRepository<Property>().GetAsync(inPropertyId);
            if (property.Price == inNewPrice)
            {
                return _mapper.Map<PropertyDto>(property);
            };
            property.Price = inNewPrice;
            await _unitOfWork.GetRepository<Property>().UpdateAsync(property);
            await _unitOfWork.Complete();
            return _mapper.Map<PropertyDto>(property);
        }

        /// <summary>
        /// Updates a property building.
        /// </summary>
        /// <param name="inPropertyDto">The updated property DTO.</param>
        /// <returns>The updated property.</returns>
        public async Task<PropertyDto> UpdatePropertyBuilding(PropertyDto inPropertyDto)
        {
            Property property = _mapper.Map<Property>(inPropertyDto);
            await _unitOfWork.GetRepository<Property>().UpdateAsync(property);
            await _unitOfWork.Complete();
            return _mapper.Map<PropertyDto>(property);
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
                return _mapper.Map<IEnumerable<PropertyDto>>(_unitOfWork.GetRepository<Property>().Entities.ToList());

            if (inFilterArgs.SortingParameters == null && !inFilterArgs.SortingParameters.Any())
            {
                inFilterArgs.SortingParameters.ToList().Add(new SortingParameters
                {
                    Direction = ESortingDirection.Ascending,
                    TargetField = "IdProperty",
                    Priority = 0
                });
            }

            PropertySpecification propertySpecification = new PropertySpecification(ValidationExpressions.CreatePropertyValidationExpression(inFilterArgs));
            properties = await _unitOfWork.GetRepository<Property>().ListByAsync(propertySpecification);

            return _mapper.Map<IEnumerable<PropertyDto>>(properties); 
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
            Property property = await _unitOfWork.GetRepository<Property>().GetAsync(inId);

            if (property == null)
            {
                throw new ArgumentException("IdProperty does not exist!");
            }

            // Return the property DTO
            return _mapper.Map<PropertyDto>(property);
        }
    }
}
