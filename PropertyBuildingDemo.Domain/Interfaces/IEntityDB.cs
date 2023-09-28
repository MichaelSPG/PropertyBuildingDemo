using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Domain.Interfaces
{
    public interface IEntityDB
    {
        long GetId();
        bool IsDeleted { get; }
        DateTime CreatedTime { get; set; }
        DateTime UpdatedTime { get; set; }
    }
}
