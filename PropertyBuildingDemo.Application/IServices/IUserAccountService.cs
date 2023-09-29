using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities.Identity;

namespace PropertyBuildingDemo.Application.IServices
{
    /// <summary>
    /// Service for managing user accounts.
    /// </summary>
    public interface IUserAccountService
    {
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userDto">The user registration data.</param>
        /// <returns>An API result containing user information if registration is successful.</returns>
        Task<ApiResult<UserDto>> RegisterUser(UserRegisterDto userDto);

        /// <summary>
        /// Gets the current user based on the provided HTTP context.
        /// </summary>
        /// <param name="httpContext">The HTTP context containing user information.</param>
        /// <returns>An API result containing user information of the current user.</returns>
        Task<ApiResult<UserDto>> GetCurrentUser(HttpContext httpContext);

        /// <summary>
        /// Finds a user by their email.
        /// </summary>
        /// <param name="email">The email address of the user to find.</param>
        /// <returns>An API result containing user information if found.</returns>
        Task<ApiResult<UserDto>> FindByEmail(string email);

        /// <summary>
        /// Finds a user by their email from a claims principal.
        /// </summary>
        /// <param name="user">The claims principal containing user information.</param>
        /// <returns>The user found in the claims principal.</returns>
        Task<AppUser> FindByEmailFromClaimPrincipal(ClaimsPrincipal user);
    }
}
