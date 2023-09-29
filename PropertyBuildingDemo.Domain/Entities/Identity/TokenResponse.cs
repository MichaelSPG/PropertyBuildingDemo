namespace PropertyBuildingDemo.Domain.Entities.Identity
{
    /// <summary>
    /// The token response, stores the token information and the refresh token also
    /// </summary>
    public class TokenResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? TokenExpiryTime { get; set; }
    }
}
