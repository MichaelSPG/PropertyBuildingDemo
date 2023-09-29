using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PropertyBuildingDemo.Domain.Entities.Enums;

namespace PropertyBuildingDemo.Domain.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            this.UserName = String.Empty;
        }
        public string DisplayName { get; set; }

        public ERoleType Role { get; set; }

        public string IdentityNumber { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
