using log4net;
using PropertyBuildingDemo.Domain.Entities.Enums;
using PropertyBuildingDemo.Domain.Interfaces;
using System.Reflection;

namespace PropertyBuildingDemo.Infrastructure.Logging
{
    public class DefaultSystemLogger : ISystemLogger
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        protected static Lazy<DefaultSystemLogger> _defaultSystemLoggerInstance = new Lazy<DefaultSystemLogger>(() => new DefaultSystemLogger());

        public static DefaultSystemLogger Instance => _defaultSystemLoggerInstance.Value;

        public void LogExceptionMessage(ELogginLevel inLogLevel, object inMessage, Exception inException)
        {
            switch (inLogLevel)
            {
                case ELogginLevel.Level_Info:
                    if (Logger.IsInfoEnabled)
                        Logger.Info(inMessage, inException);
                    break;
                case ELogginLevel.Level_Warn:
                    if (Logger.IsWarnEnabled)
                        Logger.Warn(inMessage, inException);
                    break;
                case ELogginLevel.Level_Error:
                    Logger.Error(inMessage, inException);
                    break;
                case ELogginLevel.Level_Fatal:
                    Logger.Fatal(inMessage, inException);
                    break;
            }
        }
        public void LogMessage(ELogginLevel inLogLevel, object inMessage)
        {
            switch (inLogLevel)
            {
                case ELogginLevel.Level_Info:
                    if (Logger.IsInfoEnabled)
                        Logger.Info(inMessage);
                    break;
                case ELogginLevel.Level_Warn:
                    if (Logger.IsWarnEnabled)
                        Logger.Warn(inMessage);
                    break;
                case ELogginLevel.Level_Error:
                    Logger.Error(inMessage);
                    break;
                case ELogginLevel.Level_Fatal:
                    Logger.Fatal(inMessage);
                    break;
            }
        }
    }
}
