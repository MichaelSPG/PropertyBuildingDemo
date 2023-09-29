using PropertyBuildingDemo.Application.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Common;
using Microsoft.AspNetCore.Identity;
using PropertyBuildingDemo.Application.Extensions;
using PropertyBuildingDemo.Domain.Entities.Identity;
using static Duende.IdentityServer.Models.IdentityResources;

namespace PropertyBuildingDemo.Application.Services
{
    public class UsertAccountService : IUserAccountService
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        public UsertAccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<ApiResult<UserDto>> RegisterUser(UserRegisterDto registerDto)
        {
            var user = await FindByEmail(registerDto.Email);
            if (user.Success && user.Data != null)
            {
                return ApiResult<UserDto>.FailedResult("Email has been taken");
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
            return ApiResult<UserDto>.SuccessResult(new UserDto
            {
                DisplayName = appUser.DisplayName,
                Email = appUser.Email,
                IdentityNumber = appUser.IdentityNumber
            });
        }

        public async Task<ApiResult<UserDto>> GetCurrentUser(HttpContext httpContext)
        {
            var user = await _userManager.FindByEmailFromClaimPrincipal(httpContext.User);
            return ApiResult<UserDto>.SuccessResult(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                IdentityNumber = user.IdentityNumber
            });
        }

        public async Task<ApiResult<UserDto>> FindByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return ApiResult<UserDto>.SuccessResult(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                IdentityNumber = user.IdentityNumber
            });
        }
    }
}
