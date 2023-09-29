using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities.Identity;
using PropertyBuildingDemo.Domain.Interfaces;
using PropertyBuildingDemo.Infrastructure.Config;
using PropertyBuildingDemo.Infrastructure.Data;

namespace PropertyBuildingDemo.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IOptions<ApplicationConfig> _appConfig;
        private readonly SymmetricSecurityKey _Key;
        private readonly PropertyBuildingContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        public TokenService(IConfiguration configuration, IOptions<ApplicationConfig> appConfig, PropertyBuildingContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _appConfig = appConfig;
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
            _Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfig.Value.Secret));
        }

        public async Task<ApiResult<TokenResponse>> CreateToken(TokenRequest tokenRequest)
        {
            AppUser appUser = await _userManager.FindByEmailAsync(tokenRequest.Username);

            if (appUser == null)
            {
                return await ApiResult<TokenResponse>.FailedResultAsync("User does not exist!.");
            }
            if (!appUser.IsActive)
            {
                return await ApiResult<TokenResponse>.FailedResultAsync("User is not active!.");
            }
            if (!appUser.EmailConfirmed)
            {
                return await ApiResult<TokenResponse>.FailedResultAsync("User email is not confirmed!.");
            }
            var passwordValidator = await _userManager.CheckPasswordAsync(appUser, tokenRequest.Password);
            if (!passwordValidator)
            {
                return await ApiResult<TokenResponse>.FailedResultAsync("User/pasword invalid credentials!.");
            }

            var claims = GetClaimsAsync(appUser);
            appUser.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(_appConfig.Value.ExpireInMinutes);
            appUser.RefreshToken = CreateRefreshToken();

            var token = await GenerateJwtAsync(appUser);

            var response = new TokenResponse { Token = token, RefreshToken = appUser.RefreshToken, TokenExpiryTime = appUser.RefreshTokenExpiryTime };

            _dbContext.Set<AppUser>().Update(appUser);
            await _dbContext.SaveChangesAsync();
            return ApiResult<TokenResponse>.SuccessResult(response);
        }

        private string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public Task<ApiResult<string>>  ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false, // Adjust these settings based on your token's configuration
                ValidateAudience = false,
                IssuerSigningKey = _Key,
                ValidateLifetime = true, // Ensure the token's lifetime is validated
                ClockSkew = TimeSpan.Zero // No tolerance for expiration time
            };

            if (string.IsNullOrWhiteSpace(token))
            {
                return ApiResult<string>.FailedResultAsync("No Token data has been supplied!"); // Token is not expired 
            }

            try
            {
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                if (securityToken is JwtSecurityToken jwtSecurityToken)
                {
                    // Check if the token's expiration time is in the future.
                    if (jwtSecurityToken.ValidTo < DateTime.UtcNow)
                    {
                        return ApiResult<string>.FailedResultAsync("Token has expired!"); // Token is not expired
                    }
                }
            }
            catch (Exception ex)
            {
                return ApiResult<string>.FailedResultAsync("Failed to read the token" ); // Token is not expired
            }

            return ApiResult<string>.SuccessResultAsync(); // Token is valid 
        }

        private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims, AppUser appUser)
        {
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_appConfig.Value.ExpireInMinutes),
                signingCredentials: signingCredentials,
                issuer: _appConfig.Value.Issuer, 
                audience: appUser.UserName);
            var tokenHandler = new JwtSecurityTokenHandler();
            var encryptedToken = tokenHandler.WriteToken(token);
            return encryptedToken;
        }
        private async Task<string> GenerateJwtAsync(AppUser user)
        {
            var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user), user);
            return token;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var secret = Encoding.UTF8.GetBytes(_appConfig.Value.Secret);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(AppUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            var permissionClaims = new List<Claim>();
            foreach (var role in roles)
            {
                //roleClaims.Add(new Claim(ClaimTypes.Role, role.ParseEnum<RoleType>().ToString()));
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
                var thisRole = await _roleManager.FindByNameAsync(role);
                var allPermissionsForThisRoles = await _roleManager.GetClaimsAsync(thisRole);
                permissionClaims.AddRange(allPermissionsForThisRoles);
            }
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
                    new Claim(ClaimTypes.UserData, user.IdentityNumber ?? string.Empty)
                }
                .Union(userClaims)
                .Union(roleClaims)
                .Union(permissionClaims);

            return claims;
        }
    }
}
