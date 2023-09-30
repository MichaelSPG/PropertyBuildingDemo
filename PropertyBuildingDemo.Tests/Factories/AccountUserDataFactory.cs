using PropertyBuildingDemo.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Tests.Factories
{
    public static class AccountUserDataFactory
    {

        
        public static UserRegisterDto CreateValidUserFoRegister()
        {
            return new UserRegisterDto
            {
                DisplayName = "Charles Junior",
                Email = "charles.junior@someemail.com",
                IdentificationNumber = 1230129,
                Password = "A@1213f"
            };
        }

        public static UserRegisterDto CreateInvalidUserPasswordFoRegister()
        {
            UserRegisterDto user = CreateValidUserFoRegister();
            user.Password = "1";
            return user;
        }

        public static UserDto CreateUserWithCustomProperties(string id, string username, string email)
        {
            return new UserDto
            {
                Id = id,
                DisplayName = username,
                Email = email,
            };
        }
    }
}
