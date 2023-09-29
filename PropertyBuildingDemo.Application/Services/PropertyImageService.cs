using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Domain.Interfaces;

namespace PropertyBuildingDemo.Application.Services
{
    public class PropertyImageService : IPropertyImageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PropertyImageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<PropertyImage>> AddMultipleImages(IEnumerable<PropertyImage> inPropertyImages)
        {
            return _unitOfWork.GetRepository<PropertyImage>().AddRangeAsync(inPropertyImages);
        }
    }
}
