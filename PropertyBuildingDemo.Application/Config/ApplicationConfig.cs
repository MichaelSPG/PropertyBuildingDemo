namespace PropertyBuildingDemo.Application.Config
{
    /// <summary>
    /// Represents the application configuration settings.
    /// </summary>
    public class ApplicationConfig
    {
        /// <summary>
        /// Gets or sets the base URL for the application.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the secret key used for JWT token generation.
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Gets or sets the issuer of the JWT token.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the audience of the JWT token.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Gets or sets the expiration time for JWT tokens in minutes.
        /// </summary>
        public int ExpireInMinutes { get; set; }
    }
}
