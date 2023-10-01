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
