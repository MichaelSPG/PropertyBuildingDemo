using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Domain.Interfaces;

namespace PropertyBuildingDemo.Application.Services
{
    public class PropertyTraceService : IPropertyTraceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PropertyTraceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IEnumerable<PropertyTrace>> AddMultipleTraces(IEnumerable<PropertyTrace> inPropertyTraces)
        {
            return _unitOfWork.GetRepository<PropertyTrace>().AddRangeAsync(inPropertyTraces);
        }
    }
}
