using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Domain.Common
{
    public abstract class BaseAuditableEntityDB : BaseEntityDB
    {
        public string? CreatedBy{ get; set; }
        public string? ModifiedBy { get; set; }
    }
}
