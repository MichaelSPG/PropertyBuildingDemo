using PropertyBuildingDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Domain.Interfaces
{
    public interface IPropertyService
    {
        Task<IEnumerable<Property>> GetAll();
        Task<Property>              GetAsync(long id);
        Task<Property>              AddAsync(Property entity);
        Task<Property>              UpdateAsync(Property entity);
        Task<bool>                  DeleteAsync(Property entity);
    }
}
