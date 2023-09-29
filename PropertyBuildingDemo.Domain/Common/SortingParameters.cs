using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyBuildingDemo.Domain.Entities.Enums;

namespace PropertyBuildingDemo.Domain.Common
{
    public class SortingParameters
    {
        public ESortingDirection Direction { get; set; }
        public string TargetField { get; set; }
        public int    Priority{ get; set; }
    }
}
