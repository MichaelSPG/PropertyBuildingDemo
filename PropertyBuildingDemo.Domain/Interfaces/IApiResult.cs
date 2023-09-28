using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Domain.Interfaces
{
    public interface IApiResult
    {
        IEnumerable<string> Message { get; set; }
        bool                Success { get; set; }
        int                 ResultCode { get; set; }
    }

    public interface IApiResult<TData>
    {
        TData Data { get; }
    }
}
