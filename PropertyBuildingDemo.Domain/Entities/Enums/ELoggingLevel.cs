using System.Runtime.Serialization;

namespace PropertyBuildingDemo.Domain.Entities.Enums
{
    /// <summary>
    /// Represents a logging level enum.
    /// </summary>
    public enum ELoggingLevel
    {
        /// <summary>
        /// Represents an information logging level.
        /// </summary>
        [EnumMember(Value = "Level Info")]
        Info,

        /// <summary>
        /// Represents a warning logging level.
        /// </summary>
        [EnumMember(Value = "Level Warn")]
        Warn,

        /// <summary>
        /// Represents an error logging level.
        /// </summary>
        [EnumMember(Value = "Level Error")]
        Error,

        /// <summary>
        /// Represents a fatal logging level.
        /// </summary>
        [EnumMember(Value = "Level Fatal")]
        Fatal,

        /// <summary>
        /// Represents a debug logging level.
        /// </summary>
        [EnumMember(Value = "Level Debug")]
        Debug
    }
}
