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
                .ForMember(dest => dest.PropertyImages, opt => opt.Ignore()) // Ignore PropertyImages during mapping
                .ForMember(dest => dest.PropertyTraces, opt => opt.Ignore()) // Ignore PropertyTraces during mapping
                .ReverseMap(); // Reverse mapping to map PropertyDto back to Property entity

            // PropertyImage to PropertyImageDto mapping
            CreateMap<PropertyImage, PropertyImageDto>();

            // PropertyTrace to PropertyTraceDto mapping
            CreateMap<PropertyTrace, PropertyTraceDto>();

            // PropertyImageDto to PropertyImage mapping (reverse mapping)
            CreateMap<PropertyImageDto, PropertyImage>();

            // PropertyTraceDto to PropertyTrace mapping (reverse mapping)
            CreateMap<PropertyTraceDto, PropertyTrace>();

            // Owner to OwnerDto mapping
            CreateMap<Owner, OwnerDto>()
                .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.IdOwner)); // Map IdOwner to IdOwner

            // Owner to OwnerDto mapping (no special configuration needed)
            CreateMap<Owner, OwnerDto>();
        }
    }
}
