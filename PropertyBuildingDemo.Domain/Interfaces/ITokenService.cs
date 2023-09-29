using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities.Identity;

namespace PropertyBuildingDemo.Domain.Interfaces
{
    public interface ITokenService
    {
        Task<ApiResult<TokenResponse>> CreateToken(TokenRequest inAppUser);
        Task<ApiResult<string>> ValidateToken(string token);
    }
}
