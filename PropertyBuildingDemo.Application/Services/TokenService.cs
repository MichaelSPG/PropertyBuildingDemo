using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities.Identity;
using PropertyBuildingDemo.Domain.Interfaces;
using PropertyBuildingDemo.Infrastructure.Config;
using PropertyBuildingDemo.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PropertyBuildingDemo.Application.Services
{
    /// <summary>
    /// Service for managing user tokens.
    /// </summary>
    public class TokenService : IApiTokenService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IOptions<ApplicationConfig> _appConfig;
        private readonly SymmetricSecurityKey _key;
        private readonly PropertyBuildingContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenService"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration.</param>
        /// <param name="appConfig">The application configuration options.</param>
        /// <param name="dbContext">The database context.</param>
        /// <param name="roleManager">The role manager.</param>
        /// <param name="userManager">The user manager.</param>
        public TokenService(IConfiguration configuration, IOptions<ApplicationConfig> appConfig, PropertyBuildingContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _appConfig = appConfig;
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfig.Value.Secret));
        }
        /// <summary>
        /// Creates a token for the provided token request.
        /// </summary>
        /// <param name="tokenRequest">The token request information.</param>
        /// <returns>An <see cref="ApiResult{T}"/> with the token response.</returns>
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
            //if (!appUser.EmailConfirmed)
            //{
            //    return await ApiResult<TokenResponse>.FailedResultAsync("User email is not confirmed!.");
            //}
            var passwordValidator = await _userManager.CheckPasswordAsync(appUser, tokenRequest.Password);
            if (!passwordValidator)
            {
                return await ApiResult<TokenResponse>.FailedResultAsync("User/Password invalid credentials!.");
            }

            var claims = await GetClaimsAsync(appUser);
            appUser.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(_appConfig.Value.ExpireInMinutes);
            appUser.RefreshToken = CreateRefreshToken();

            var token = await GenerateJwtAsync(appUser);
            var response = new TokenResponse { Token = token, RefreshToken = appUser.RefreshToken, TokenExpiryTime = appUser.RefreshTokenExpiryTime };

            _dbContext.Set<AppUser>().Update(appUser);
            await _dbContext.SaveChangesAsync();

            return await ApiResult<TokenResponse>.SuccessResultAsync(response);
        }

        private string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        /// <summary>
        /// Validates a JWT token.
        /// </summary>
        /// <param name="token">The JWT token to validate.</param>
        /// <returns>An <see cref="ApiResult{T}"/> indicating the token's validation result.</returns>
        public Task<ApiResult<string>>  ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false, // Adjust these settings based on your token's configuration
                ValidateAudience = false,
                IssuerSigningKey = _key,
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
        /// <summary>
        /// Generates an encrypted JWT token.
        /// </summary>
        /// <param name="signingCredentials">The signing credentials for the token.</param>
        /// <param name="claims">The claims to include in the token.</param>
        /// <param name="appUser">The user for whom the token is generated.</param>
        /// <returns>The encrypted JWT token.</returns>
        private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims, AppUser appUser)
        {
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(_appConfig.Value.ExpireInMinutes),
                signingCredentials: signingCredentials,
                issuer: _appConfig.Value.Issuer, 
                audience: appUser.UserName);
            var tokenHandler = new JwtSecurityTokenHandler();
            var encryptedToken = tokenHandler.WriteToken(token);
            return encryptedToken;
        }
        /// <summary>
        /// Generates a JWT token asynchronously for the given user.
        /// </summary>
        /// <param name="user">The user for whom the token is generated.</param>
        /// <returns>The generated JWT token.</returns>
        private async Task<string> GenerateJwtAsync(AppUser user)
        {
            var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user), user);
            return token;
        }
        /// <summary>
        /// Gets the signing credentials for generating JWT tokens.
        /// </summary>
        /// <returns>The signing credentials.</returns>
        private SigningCredentials GetSigningCredentials()
        {
            var secret = Encoding.UTF8.GetBytes(_appConfig.Value.Secret);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }
        /// <summary>
        /// Gets a list of claims for the given user, including user claims, role claims, and permission claims.
        /// </summary>
        /// <param name="user">The user for whom the claims are generated.</param>
        /// <returns>The list of claims.</returns>
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
                    new Claim(ClaimTypes.UserData, user.IdentificationNumber.ToString())
                }
                .Union(userClaims)
                .Union(roleClaims)
                .Union(permissionClaims);

            return claims;
        }
    }
}
