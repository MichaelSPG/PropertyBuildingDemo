using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Application.IServices;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities.Identity;
using System.Security.Claims;

namespace PropertyBuildingDemo.Application.Services
{
    /// <summary>
    /// Service for user account-related operations.
    /// </summary>
    public class UserAccountService : IUserAccountService
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccountService"/> class.
        /// </summary>
        /// <param name="userManager">The UserManager for managing users.</param>
        /// <param name="signInManager">The SignInManager for user sign-in.</param>
        public UserAccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Registers a new user with the provided registration information.
        /// </summary>
        /// <param name="registerDto">The registration information.</param>
        /// <returns>An <see cref="ApiResult{T}"/> with user registration results.</returns>
        public async Task<ApiResult<UserDto>> RegisterUser(UserRegisterDto registerDto)
        {
            var user = await FindByEmail(registerDto.Email);
            if (user.Success && user.Data != null)
            {
                return await ApiResult<UserDto>.FailedResultAsync("Email has been taken");
            }
            var appUser = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email,
            };

            var result = await _userManager.CreateAsync(appUser, registerDto.Password);
            if (!result.Succeeded)
            {
                return await ApiResult<UserDto>.FailedResultAsync(result.Errors.Select(x => x.Description).ToList());
            }
            return await ApiResult<UserDto>.SuccessResultAsync(new UserDto
            {
                DisplayName = appUser.DisplayName,
                Email = appUser.Email,
                IdentityNumber = appUser.IdentityNumber
            });
        }

        /// <summary>
        /// Finds a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user to find.</param>
        /// <returns>An <see cref="ApiResult{T}"/> with user information if found.</returns>
        public async Task<ApiResult<UserDto>> FindByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null ?  ApiResult<UserDto>.SuccessResult(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                IdentityNumber = user.IdentityNumber
            }) : ApiResult<UserDto>.FailedResult();
        }

        /// <summary>
        /// Finds a user by their email address from a ClaimsPrincipal.
        /// </summary>
        /// <param name="user">The ClaimsPrincipal containing user claims.</param>
        /// <returns>The user with matching email address.</returns>
        public async Task<AppUser> FindByEmailFromClaimPrincipal(ClaimsPrincipal user)
        {
            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            return await _userManager.Users.SingleOrDefaultAsync(x => x.Email == email);
        }

        /// <summary>
        /// Gets the current user from an HTTP context.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>An <see cref="ApiResult{T}"/> with the current user's information.</returns>
        public async Task<ApiResult<UserDto>> GetCurrentUser(HttpContext httpContext)
        {
            var user = await FindByEmailFromClaimPrincipal(httpContext.User);
            return ApiResult<UserDto>.SuccessResult(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                IdentityNumber = user.IdentityNumber
            });
        }
    }
}
