using PropertyBuildingDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Application.Dto
{
    public class PropertyImageDto
    {
        public long IdPropertyImage { get; set; }

        [ForeignKey(nameof(Property))] public long IdProperty { get; set; }

        [Required] public byte[] File { get; set; }
        public bool Enabled { get; set; }
    }
}
