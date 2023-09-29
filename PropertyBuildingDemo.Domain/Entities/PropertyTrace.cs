using PropertyBuildingDemo.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyBuildingDemo.Domain.Entities
{
    /// <summary>
    /// Represents a property trace entity with audit and trace information.
    /// </summary>
    public class PropertyTrace : BaseAuditableEntityDb
    {
        /// <summary>
        /// Gets or sets the primary key of this table (PropertyTrace).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdPropertyTrace { get; set; }

        /// <summary>
        /// Gets or sets the date of the property sale.
        /// </summary>
        public DateTime? DateSale { get; set; }

        /// <summary>
        /// Gets or sets the name associated with this property trace.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of this property trace.
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; set; }

        /// <summary>
        /// Gets or sets the tax associated with this property trace.
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Tax { get; set; }

        /// <summary>
        /// Gets or sets the foreign key to the associated property.
        /// </summary>
        [ForeignKey(nameof(Property))]
        public long IdProperty { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the associated property.
        /// </summary>
        public virtual Property Property { get; set; }

        /// <summary>
        /// Implementation of the GetId() method, considering the different column name ([Key]) for this entity.
        /// </summary>
        /// <returns>The Id of this property trace.</returns>
        public override long GetId()
        {
            return IdPropertyTrace;
        }
    }
}
