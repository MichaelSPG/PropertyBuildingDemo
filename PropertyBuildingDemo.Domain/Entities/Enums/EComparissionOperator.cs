using System.ComponentModel;
using System.Xml.Serialization;

namespace PropertyBuildingDemo.Domain.Entities.Enums
{
    
    /// <summary>
    /// Represents a generic comparison operator enum.
    /// </summary>
    public enum EComparisionOperator
    {
        [Description("Contains")]
        [XmlEnum("Contains")]
        Contains,

        [Description("Not Contains")]
        [XmlEnum("NotContains")]
        NotContains,

        [Description("Less Than")]
        [XmlEnum("LessThan")]
        LessThan,

        [Description("Less Than or Equal")]
        [XmlEnum("LessThanOrEqual")]
        LessThanEqual,

        [Description("Greater Than")]
        [XmlEnum("GreaterThan")]
        GreaterThan,

        [Description("Greater Than or Equal")]
        [XmlEnum("GreaterThanOrEqual")]
        GreaterThanEqual,

        [Description("Not Equal")]
        [XmlEnum("NotEqual")]
        NotEqual,

        [Description("Equal")]
        [XmlEnum("Equal")]
        Equal,

        [Description("Starts With")]
        [XmlEnum("StartsWith")]
        StartsWith,

        [Description("Ends With")]
        [XmlEnum("EndsWith")]
        EndsWith
    }
}