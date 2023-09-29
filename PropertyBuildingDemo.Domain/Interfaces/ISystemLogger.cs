using PropertyBuildingDemo.Domain.Entities.Enums;

namespace PropertyBuildingDemo.Domain.Interfaces
{
    /// <summary>
    /// Represents a logger interface for system logging.
    /// </summary>
    public interface ISystemLogger
    {
        /// <summary>
        /// Logs a message with the specified log level.
        /// </summary>
        /// <param name="inLogLevel">The log level of the message.</param>
        /// <param name="inMessage">The message to log.</param>
        void LogMessage(ELoggingLevel inLogLevel, object inMessage);

        /// <summary>
        /// Logs a message with the specified log level and exception.
        /// </summary>
        /// <param name="inLogLevel">The log level of the message.</param>
        /// <param name="inMessage">The message to log.</param>
        /// <param name="inException">The exception associated with the message.</param>
        void LogExceptionMessage(ELoggingLevel inLogLevel, object inMessage, Exception inException);
    }
}
