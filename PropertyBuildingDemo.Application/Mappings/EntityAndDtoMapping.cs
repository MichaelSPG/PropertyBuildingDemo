using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Entities;

namespace PropertyBuildingDemo.Application.Mappings
{
    /// <summary>
    /// Class to map from Entity (table representation) to EntityDto
    /// </summary>
    public class EntityAndDtoMapping : Profile
    {
        public EntityAndDtoMapping()
        {
            CreateMap<Property, PropertyDto>()
                .ForMember(dest => dest.PropertyImages, opt => opt.Ignore())
                .ForMember(dest => dest.PropertyTraces, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<PropertyImage, PropertyImageDto>();
            CreateMap<PropertyTrace, PropertyTraceDto>();
            CreateMap<PropertyImageDto, PropertyImage>();
            CreateMap<PropertyTraceDto, PropertyTrace>();
            CreateMap<Owner, OwnerDto>() 
                .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.IdOwner)); // Map IdOwner to IdOwner

            CreateMap<Owner, OwnerDto>();
        }
    }
}
