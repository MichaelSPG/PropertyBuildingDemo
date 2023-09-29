using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Application.Dto
{
    public  class UserDto
    {
        public string IdentityNumber { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }

    }
}
