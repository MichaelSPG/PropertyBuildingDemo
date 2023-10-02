using AutoMapper;
using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Entities;

namespace PropertyBuildingDemo.Application.Mappings
{
    /// <summary>
    /// AutoMapper profile for mapping between entities and DTOs.
    /// </summary>
    public class EntityAndDtoMapping : Profile
    {
        public EntityAndDtoMapping()
        {
            // Property to PropertyDto mapping
            CreateMap<Property, PropertyDto>()
                .ForMember(dest => dest.PropertyImages, opt => opt.MapFrom(src => src.PropertyImages))
                .ForMember(dest => dest.PropertyTraces, opt => opt.MapFrom(src => src.PropertyTraces))
                .ReverseMap(); // Reverse mapping to map PropertyDto back to Property entity

            // PropertyImage to PropertyImageDto mapping
            CreateMap<PropertyImage, PropertyImageDto>().ReverseMap();

            // PropertyTrace to PropertyTraceDto mapping
            CreateMap<PropertyTrace, PropertyTraceDto>().ReverseMap();

            // Owner to OwnerDto mapping
            CreateMap<Owner, OwnerDto>()
                .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.IdOwner))
                .ReverseMap(); ; // Map IdOwner to IdOwner
        }
    }
}