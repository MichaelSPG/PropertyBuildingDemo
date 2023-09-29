using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PropertyBuildingDemo.Application.Dto
{
    public class PropertyDto
    {
        public PropertyDto()
        {
            PropertyImages = new List<PropertyImageDto>();
            PropertyTraces = new List<PropertyTraceDto>();
        }
        public long IdProperty { get; set; }
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
        [StringLength(40)]
        [Required]
        public string Address { get; set; }
        [Required]
        public decimal Price { get; set; }
        public string InternalCode { get; set; }
        public long Year{ get; set; }
        [Required]
        public long IdOwner{ get; set; }
        public IEnumerable<PropertyImageDto> PropertyImages { get; set; }
        public IEnumerable<PropertyTraceDto> PropertyTraces { get; set; }
    }
}
