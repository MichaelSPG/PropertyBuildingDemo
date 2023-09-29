using System.ComponentModel;
using System.Xml.Serialization;

namespace PropertyBuildingDemo.Domain.Entities.Enums
{
    /// <summary>
    /// Represents a generic role type enum.
    /// </summary>
    public enum ERoleType
    {
        /// <summary>
        /// Represents a basic role.
        /// </summary>
        [XmlEnum("RoleBasic")]
        [Description("Basic role")]
        Basic,

        /// <summary>
        /// Represents a customer role.
        /// </summary>
        [XmlEnum("RoleCustomer")]
        [Description("Customer role")]
        Customer,

        /// <summary>
        /// Represents a seller role.
        /// </summary>
        [XmlEnum("RoleSeller")]
        [Description("Seller role")]
        Seller,

        /// <summary>
        /// Represents a property owner role.
        /// </summary>
        [XmlEnum("RolePropertyOwner")]
        [Description("Property owner role")]
        PropertyOwner,

        /// <summary>
        /// Represents an admin role.
        /// </summary>
        [XmlEnum("RoleAdmin")]
        [Description("Admin role")]
        Admin,
    }
}
