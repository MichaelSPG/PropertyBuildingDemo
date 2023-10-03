using log4net;
using PropertyBuildingDemo.Domain.Entities.Enums;
using PropertyBuildingDemo.Domain.Interfaces;
using System.Reflection;

namespace PropertyBuildingDemo.Infrastructure.Logging
{
    /// <summary>
    /// Represents a default system logger implementation using log4net.
    /// </summary>
    public class DefaultSystemLogger : ISystemLogger
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        protected static Lazy<DefaultSystemLogger> DefaultSystemLoggerInstance = new Lazy<DefaultSystemLogger>(() => new DefaultSystemLogger());

        /// <summary>
        /// Gets the singleton instance of the <see cref="DefaultSystemLogger"/>.
        /// </summary>
        public static DefaultSystemLogger Instance => DefaultSystemLoggerInstance.Value;

        /// <summary>
        /// Logs an exception message with the specified logging level.
        /// </summary>
        /// <param name="inLogLevel">The logging level.</param>
        /// <param name="inMessage">The message to log.</param>
        /// <param name="inException">The exception to log.</param>
        public void LogExceptionMessage(ELoggingLevel inLogLevel, object inMessage, Exception inException)
        {
            switch (inLogLevel)
            {
                case ELoggingLevel.Info:
                    if (Logger.IsInfoEnabled)
                        Logger.Info(inMessage, inException);
                    break;
                case ELoggingLevel.Warn:
                    if (Logger.IsWarnEnabled)
                        Logger.Warn(inMessage, inException);
                    break;
                case ELoggingLevel.Error:
                    Logger.Error(inMessage, inException);
                    break;
                case ELoggingLevel.Fatal:
                    Logger.Fatal(inMessage, inException);
                    break;
                case ELoggingLevel.Debug:
                    Logger.Debug(inMessage, inException);
                    break;
            }
        }

        /// <summary>
        /// Logs a message with the specified logging level.
        /// </summary>
        /// <param name="inLogLevel">The logging level.</param>
        /// <param name="inMessage">The message to log.</param>
        public void LogMessage(ELoggingLevel inLogLevel, object inMessage)
        {
            switch (inLogLevel)
            {
                case ELoggingLevel.Info:
                    if (Logger.IsInfoEnabled)
                        Logger.Info(inMessage);
                    break;
                case ELoggingLevel.Warn:
                    if (Logger.IsWarnEnabled)
                        Logger.Warn(inMessage);
                    break;
                case ELoggingLevel.Error:
                    Logger.Error(inMessage);
                    break;
                case ELoggingLevel.Fatal:
                    Logger.Fatal(inMessage);
                    break;
                case ELoggingLevel.Debug:
                    Logger.Debug(inMessage);
                    break;
            }
        }
    }
}
