using log4net;
using PropertyBuildingDemo.Domain.Entities.Enums;
using PropertyBuildingDemo.Domain.Interfaces;
using System.Reflection;

namespace PropertyBuildingDemo.Infrastructure.Logging
{
    public class DefaultSystemLogger : ISystemLogger
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected static Lazy<DefaultSystemLogger> _defaultSystemLogger = new Lazy<DefaultSystemLogger>(() => new DefaultSystemLogger());

        public static DefaultSystemLogger Instance
        {
            get { return _defaultSystemLogger.Value; }
        }

        public void LogExceptionMessage(eLogginLevel InLogLevel, object InMessage, Exception InException)
        {
            switch (InLogLevel)
            {
                case eLogginLevel.Level_Info:
                    if (_logger.IsInfoEnabled)
                        _logger.Info(InMessage, InException);
                    break;
                case eLogginLevel.Level_Warn:
                    if (_logger.IsWarnEnabled)
                        _logger.Warn(InMessage, InException);
                    break;
                case eLogginLevel.Level_Error:
                    _logger.Error(InMessage, InException);
                    break;
                case eLogginLevel.Level_Fatal:
                    _logger.Fatal(InMessage, InException);
                    break;
            }
        }
        public void LogMessage(eLogginLevel InLogLevel, object InMessage)
        {
            switch (InLogLevel)
            {
                case eLogginLevel.Level_Info:
                    if (_logger.IsInfoEnabled)
                        _logger.Info(InMessage);
                    break;
                case eLogginLevel.Level_Warn:
                    if (_logger.IsWarnEnabled)
                        _logger.Warn(InMessage);
                    break;
                case eLogginLevel.Level_Error:
                    _logger.Error(InMessage);
                    break;
                case eLogginLevel.Level_Fatal:
                    _logger.Fatal(InMessage);
                    break;
            }
        }
    }
}
