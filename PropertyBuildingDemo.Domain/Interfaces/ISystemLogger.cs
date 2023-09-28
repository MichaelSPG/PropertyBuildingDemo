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
        void LogMessage(eLogginLevel InLogLevel, object InMessage);
        void LogExceptionMessage(eLogginLevel InLogLevel, object InMessage, Exception InException);
    }
}
