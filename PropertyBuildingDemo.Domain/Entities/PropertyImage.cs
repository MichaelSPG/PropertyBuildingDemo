using PropertyBuildingDemo.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PropertyBuildingDemo.Domain.Entities
{
    /// <summary>
    /// Represents a property image entity with audit and image information.
    /// </summary>
    public class PropertyImage : BaseAuditableEntityDb
    {
        /// <summary>
        /// Gets or sets the primary key of this table (PropertyImage).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdPropertyImage { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the associated property.
        /// </summary>
        [ForeignKey(nameof(Property))]
        public long IdProperty { get; set; }

        /// <summary>
        /// Gets or sets the binary data of the image file.
        /// </summary>
        [Required]
        public byte[] File { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the image is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the associated property.
        /// </summary>
        [JsonIgnore]
        public virtual Property Property { get; set; }

        /// <summary>
        /// Implementation of the GetId() method, considering the different column name ([Key]) for this entity.
        /// </summary>
        /// <returns>The Id of this property image.</returns>
        public override long GetId()
        {
            return IdPropertyImage;
        }
    }
}
