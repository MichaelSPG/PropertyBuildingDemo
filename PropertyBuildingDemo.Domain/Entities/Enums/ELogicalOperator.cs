using System.Runtime.Serialization;

namespace PropertyBuildingDemo.Domain.Entities.Enums
{
    /// <summary>
    /// Represents a generic logical operator enum.
    /// </summary>
    public enum ELogicalOperator
    {
        [EnumMember(Value = "And")]
        And,

        [EnumMember(Value = "Or")]
        Or
    }
}