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
    public class Owner : BaseAuditableEntityDB
    {
        /// <summary>
        /// The primary key of this table (Owner)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long         IdOwner { get; set; }

        [Required]
        public string       Name { get; set; }

        [Required]
        public string       Address { get; set; }

        [Required]
        public byte[]       Photo { get; set; }
        public DateTime     BirthDay { get; set; }

        /// <summary>
        /// Implementation of GetId(), due to diferent names of columns ([Key]). for this is IdOwner.
        /// </summary>
        /// <returns>the Id of this property</returns>
        public override long GetId()
        {
            return IdOwner;
        }
    }
}
