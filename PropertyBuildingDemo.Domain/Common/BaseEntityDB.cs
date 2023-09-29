using PropertyBuildingDemo.Domain.Interfaces;

namespace PropertyBuildingDemo.Domain.Common
{
    /// <summary>
    /// Represents a base entity in the database with common properties.
    /// </summary>
    public abstract class BaseEntityDb : IEntityDb
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEntityDb"/> class.
        /// </summary>
        protected BaseEntityDb()
        {
            CreatedTime = DateTime.Now;
            UpdatedTime = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is marked as deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the entity was created.
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the entity was last updated.
        /// </summary>
        public DateTime UpdatedTime { get; set; }

        /// <summary>
        /// Gets the unique identifier of the entity.
        /// </summary>
        /// <returns>The unique identifier of the entity.</returns>
        public abstract long GetId();
    }
}
