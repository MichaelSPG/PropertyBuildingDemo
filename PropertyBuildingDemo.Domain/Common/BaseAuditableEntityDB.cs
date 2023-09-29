namespace PropertyBuildingDemo.Domain.Common
{
    /// <summary>
    /// Represents a base auditable entity in the database with created and modified information.
    /// </summary>
    public abstract class BaseAuditableEntityDb : BaseEntityDb
    {
        /// <summary>
        /// Gets or sets the username of the user who created the entity.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the username of the user who last modified the entity.
        /// </summary>
        public string ModifiedBy { get; set; }
    }
}
