namespace PropertyBuildingDemo.Domain.Interfaces
{
    /// <summary>
    /// Represents the current user service interface for accessing user-related information.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Gets the user ID of the current user.
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// Gets the username of the current user.
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// Gets or sets a list of claims associated with the current user.
        /// </summary>
        public List<KeyValuePair<string, string>> Claims { get; set; }

        /// <summary>
        /// Gets the role of the current user.
        /// </summary>
        public string Role { get; }
    }
}
