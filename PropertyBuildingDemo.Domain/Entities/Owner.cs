using PropertyBuildingDemo.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyBuildingDemo.Domain.Entities
{
    /// <summary>
    /// Represents an owner entity with audit and property information.
    /// </summary>
    public class Owner : BaseAuditableEntityDb
    {
        /// <summary>
        /// Gets or sets the primary key of this table (Owner).
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IdOwner { get; set; }

        /// <summary>
        /// Gets or sets the name of the owner.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the address of the owner.
        /// </summary>
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the photo of the owner as a byte array.
        /// </summary>
        [Required]
        public byte[] Photo { get; set; }

        /// <summary>
        /// Gets or sets the birth date of the owner.
        /// </summary>
        public DateTime BirthDay { get; set; }

        /// <summary>
        /// Implementation of the GetId() method, considering the different column name ([Key]) for this entity.
        /// </summary>
        /// <returns>The Id of this owner.</returns>
        public override long GetId()
        {
            return IdOwner;
        }
    }
}
