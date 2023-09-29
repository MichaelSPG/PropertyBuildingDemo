using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities.Identity;

namespace PropertyBuildingDemo.Domain.Interfaces
{
    /// <summary>
    /// Represents a service interface for token management.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Creates a token based on the provided user credentials.
        /// </summary>
        /// <param name="inAppUser">The user credentials for token creation.</param>
        /// <returns>A task representing the asynchronous operation and the token response.</returns>
        Task<ApiResult<TokenResponse>> CreateToken(TokenRequest inAppUser);

        /// <summary>
        /// Validates the provided token.
        /// </summary>
        /// <param name="token">The token to validate.</param>
        /// <returns>A task representing the asynchronous operation and the validation result.</returns>
        Task<ApiResult<string>> ValidateToken(string token);
    }
}
