using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Entities.Identity;
using PropertyBuildingDemo.Tests.Helpers;

namespace PropertyBuildingDemo.Tests.Factories
{
    /// <summary>
    /// Factory class for creating token-related objects for testing purposes.
    /// </summary>
    public static class TokenDataFactory
    {
        /// <summary>
        /// Creates an expired <see cref="TokenResponse"/>.
        /// </summary>
        /// <returns>An expired <see cref="TokenResponse"/>.</returns>
        public static TokenResponse CreateExpiredTokenResponse()
        {
            return new TokenResponse()
            {
                RefreshToken = Guid.NewGuid().ToString(),
                Token = TestConstants.ValidExpiredToken,
                TokenExpiryTime = DateTime.UtcNow,
            };
        }

        /// <summary>
        /// Creates a corrupted <see cref="TokenResponse"/>.
        /// </summary>
        /// <returns>A corrupted <see cref="TokenResponse"/>.</returns>
        public static TokenResponse CreateCorruptedTokenResponse()
        {
            return new TokenResponse()
            {
                RefreshToken = Guid.NewGuid().ToString(),
                Token = "eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWl",
                TokenExpiryTime = null,
            };
        }

        /// <summary>
        /// Creates a custom <see cref="TokenRequest"/> with the specified username and password.
        /// </summary>
        /// <param name="userName">The username for the token request.</param>
        /// <param name="password">The password for the token request.</param>
        /// <returns>A custom <see cref="TokenRequest"/>.</returns>
        public static TokenRequest CreateTokenRequestCustom(string userName = "", string password = "")
        {
            return new TokenRequest()
            {
                Username = userName,
                Password = password
            };
        }

        /// <summary>
        /// Creates a <see cref="TokenRequest"/> from user registration data.
        /// </summary>
        /// <param name="user">The user registration data.</param>
        /// <returns>A <see cref="TokenRequest"/> created from the user registration data.</returns>
        public static TokenRequest CreateTokenRequestFromUserData(UserRegisterDto user)
        {
            return new TokenRequest()
            {
                Username = user.Email,
                Password = user.Password,
            };
        }
    }
}
