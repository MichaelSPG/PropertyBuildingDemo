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
        public string?      Name { get; set; }
        public string?      Address { get; set; }
        public decimal?     Price { get; set; }
        public string?      InternalCode { get; set; }
        public short?       Year { get; set; }

        [ForeignKey(nameof(IdOwner))]
        public long IdOwner { get; set; }

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
