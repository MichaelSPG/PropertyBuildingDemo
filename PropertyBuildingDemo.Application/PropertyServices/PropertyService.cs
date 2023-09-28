using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Application.PropertyServices
{
    public class PropertyService : IEntityService<Property>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PropertyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Property> AddAsync(Property entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Property entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Property>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Property> GetAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<Property> UpdateAsync(Property entity)
        {
            throw new NotImplementedException();
        }
    }
}
