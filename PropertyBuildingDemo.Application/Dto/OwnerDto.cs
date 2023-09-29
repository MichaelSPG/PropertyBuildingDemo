using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PropertyBuildingDemo.Application.Dto
{
    public class OwnerDto
    {
        public long IdOwner { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }
        
        [Required]
        public byte[] Photo { get; set; }
        public DateTime BirthDay { get; set; }
    }
}
