using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Common;

namespace PropertyBuildingDemo.Application.IServices
{
    public interface IUserAccountService
    {
        Task<ApiResult<UserDto>> RegisterUser(UserRegisterDto userDto);
        Task<ApiResult<UserDto>> GetCurrentUser(HttpContext httpContext);
        Task<ApiResult<UserDto>> FindByEmail(string email);
    }
}
