using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Tests.Factories
{
    public interface IDataFactory<TEntityDto>
    {
        TEntityDto CreateValidEntityDto();
        IEnumerable<TEntityDto> CreateValidEntityDtoList(int count, long refId = 0);
    }
}
