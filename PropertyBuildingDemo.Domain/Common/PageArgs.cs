using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Domain.Common
{
    public class PageArgs
    {
        public PageArgs()
        {
            PageIndex = 0;
            PageSize = 15;
        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
