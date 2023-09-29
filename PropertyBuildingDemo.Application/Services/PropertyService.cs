using AutoMapper;
using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Application.Services;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Domain.Entities.Enums;
using PropertyBuildingDemo.Domain.Interfaces;
using PropertyBuildingDemo.Domain.Specifications;
using System.Linq.Expressions;
using PropertyBuildingDemo.Application.Helpers;

namespace PropertyBuildingDemo.Application.PropertyServices
{
    public class PropertyService : IPropertyBuildingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPropertyImageService _inImageService;
        private readonly IPropertyTraceService _inPropertyTraceService;

        public PropertyService(IUnitOfWork unitOfWork, IMapper mapper, IPropertyImageService inImageService, IPropertyTraceService inPropertyTraceService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _inImageService = inImageService;
            _inPropertyTraceService = inPropertyTraceService;
        }

        public async Task<Property> CreatePropertyBuilding(PropertyDto inPropertyDto)
        {
            Owner owner = await _unitOfWork.GetRepository<Owner>().GetAsync(inPropertyDto.IdOwner);

            if (owner == null)
            {
                throw new ArgumentException("IdOwner does not exist!");
            }

            await _unitOfWork.BeginTransaction();

            Property newProperty = _mapper.Map<Property>(inPropertyDto);
            newProperty = await _unitOfWork.GetRepository<Property>().AddAsync(newProperty);
            await _unitOfWork.Complete(inFinishTransaction:false);
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

            return newProperty;
        }

        public async Task<PropertyImage> AddImageFromProperty(PropertyImageDto inImageDto)
        {
            PropertyImage newPropertyImage = _mapper.Map<PropertyImage>(inImageDto);
            await _unitOfWork.GetRepository<PropertyImage>().AddAsync(newPropertyImage);
            await _unitOfWork.Complete();
            return newPropertyImage;
        }

        public async Task<Property> ChangePrice(long inPropertyId, decimal inNewPrice)
        {
            Property property = await _unitOfWork.GetRepository<Property>().GetAsync(inPropertyId);
            if (property.Price == inNewPrice)
            {
                return property;
            };
            property.Price = inNewPrice;
            await _unitOfWork.GetRepository<Property>().UpdateAsync(property);
            await _unitOfWork.Complete();
            return property;
        }

        public async Task<Property> UpdatePropertyBuilding(PropertyDto inPropertyDto)
        {
            Property property = _mapper.Map<Property>(inPropertyDto);
            await _unitOfWork.GetRepository<Property>().UpdateAsync(property);
            await _unitOfWork.Complete();
            return property;
        }

        public async Task<IEnumerable<Property>> FilterPropertyBuildings(DefaultQueryFilterArgs inFilterArgs)
        {
            IEnumerable<Property> properties = new List<Property>();
            if (inFilterArgs == null)
                return _unitOfWork.GetRepository<Property>().Entities.ToList();

            if (inFilterArgs.SortingParameters == null && !inFilterArgs.SortingParameters.Any())
            {
                inFilterArgs.SortingParameters.ToList().Add(new SortingParameters
                {
                    Direction = ESortingDirection.DirectionAsc,
                    TargetField = "IdProperty",
                    Priority = 0
                });
            }

            PropertySpecification propertySpecification = new PropertySpecification(ValidationExpressions.CreatePropertyValidationExpression(inFilterArgs));
            properties = await _unitOfWork.GetRepository<Property>().ListByAsync(propertySpecification);

            return properties;


        }
    }
}
