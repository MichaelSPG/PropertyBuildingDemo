using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyBuildingDemo.Domain.Entities;

namespace PropertyBuildingDemo.Domain.Interfaces
{
    public interface IPropertyImageService
    {
        Task<IEnumerable<PropertyImage>> AddMultipleImages(IEnumerable<PropertyImage> inPropertyImages);
    }
}
