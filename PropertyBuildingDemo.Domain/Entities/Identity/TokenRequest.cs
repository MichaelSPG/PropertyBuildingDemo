using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Domain.Entities.Identity
{
    public class TokenRequest
    {
        public string Username { get; set; }
        public string Password{ get; set; }
    }
}
