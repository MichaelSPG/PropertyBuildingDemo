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
    public class PropertyImage : BaseAuditableEntityDB
    {
        /// <summary>
        /// The primary key of this table (PropertyImage)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long         IdPropertyImage { get; set; }

        [ForeignKey(nameof(Property))]
        public long         IdProperty { get; set; }

        [Required]
        public byte[]       File { get; set; }
        public bool         Enabled{ get; set; }

        public Property     Property { get; set; }
        /// <summary>
        /// Implementation of GetId(), due to difference of names of columns ([Key]). for this is PropertyImage.
        /// </summary>
        /// <returns>the Id of this property</returns>
        public override long GetId()
        {
            return IdPropertyImage;
        }
    }
}
