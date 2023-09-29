using PropertyBuildingDemo.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Domain.Interfaces
{
    public interface ISystemLogger
    {
        void LogMessage(ELogginLevel InLogLevel, object InMessage);
        void LogExceptionMessage(ELogginLevel InLogLevel, object InMessage, Exception InException);
    }
}
