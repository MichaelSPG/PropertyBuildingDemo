using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Application.Dto
{
    public  class UserRegisterDto
    {
        public int Id { get; set; }

        [Required]
        public int IdentificationNumber { get; set; }

        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [PasswordPropertyText]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$",
            ErrorMessage = "Passord Must have Atleast 1 Lower 1 Uppera nd 1 Special Character")]
        public string Password { get; set; }

    }
}
