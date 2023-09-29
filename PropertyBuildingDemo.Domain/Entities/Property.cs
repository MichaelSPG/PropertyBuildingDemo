using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Domain.Entities
{
    public class Property : BaseAuditableEntityDB
    {
        /// <summary>
        /// The primary key of this table (Property)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long         IdProperty { get; set; }

        [Required]
        public string       Name { get; set; }

        [Required]
        public string       Address { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal      Price { get; set; }

        [Required]
        public string       InternalCode { get; set; }
        public short        Year { get; set; }

        [ForeignKey(nameof(Owner))]
        public long         IdOwner { get; set; }

        public Owner        Owner { get; set; }

        /// <summary>
        /// Implementation of GetId(), due to diferent names of columns ([Key]). for this is IdProperty.
        /// </summary>
        /// <returns>the Id of this property</returns>
        public override long GetId()
        {
            return IdProperty;
        }
    }
}
