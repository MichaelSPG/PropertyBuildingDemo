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
    public class PropertyTraceDto
    {
        public long     IdPropertyTrace { get; set; }
        public DateTime DateSale { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Value { get; set; }

        public decimal Tax { get; set; }
        public long IdProperty { get; set; }
    }
}
