using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities;

namespace PropertyBuildingDemo.Application.Services
{
    public interface IPropertyBuildingService
    {
        Task<Property> CreatePropertyBuilding(PropertyDto inPropertyDto);
        Task<PropertyImage> AddImageFromProperty(PropertyImageDto inImageDto);
        Task<Property> ChangePrice(long inIdProperty, decimal inNewPrice);
        Task<Property> UpdatePropertyBuilding(PropertyDto inPropertyDto);
        Task<IEnumerable<Property>> FilterPropertyBuildings(DefaultQueryFilterArgs inFilterArgs);

    }
}
