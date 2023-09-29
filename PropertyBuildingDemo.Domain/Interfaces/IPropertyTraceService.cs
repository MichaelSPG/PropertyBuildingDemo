using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyBuildingDemo.Domain.Entities;

namespace PropertyBuildingDemo.Domain.Interfaces
{
    public interface IPropertyTraceService
    {
        Task<IEnumerable<PropertyTrace>> AddMultipleTraces(IEnumerable<PropertyTrace> inPropertyTraces);
    }
}
