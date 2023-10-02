using PropertyBuildingDemo.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace PropertyBuildingDemo.Domain.Entities
{
    /// <summary>
    /// Represents a property entity with audit and property information.
    /// </summary>
    public class Property : BaseAuditableEntityDb
    {
        /// <summary>
        /// Gets or sets the primary key of this table (Property).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdProperty { get; set; }

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the address of the property.
        /// </summary>
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the price of the property.
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the internal code of the property.
        /// </summary>
        [Required]
        public string InternalCode { get; set; }

        /// <summary>
        /// Gets or sets the year of the property.
        /// </summary>
        public short Year { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the owner of the property.
        /// </summary>
        [ForeignKey(nameof(Owner))]
        public long IdOwner { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the owner of the property.
        /// </summary>
        public virtual Owner Owner { get; set; }


        [NotMapped]
        public virtual ICollection<PropertyImage> PropertyImages{ get; set; }
        [NotMapped]
        public virtual ICollection<PropertyTrace> PropertyTraces { get; set; }

        /// <summary>
        /// Implementation of the GetId() method, considering the different column name ([Key]) for this entity.
        /// </summary>
        /// <returns>The Id of this property.</returns>
        public override long GetId()
        {
            return IdProperty;
        }
    }
}
