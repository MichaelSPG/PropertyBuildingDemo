using Microsoft.AspNetCore.Http;
using PropertyBuildingDemo.Domain.Interfaces;
using System.Security.Claims;

namespace PropertyBuildingDemo.Application.Services
{
    /// <summary>
    /// Service for managing the current user.
    /// </summary>
    public class CurentUserService : ICurrentUserService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurentUserService"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor for accessing the current HTTP context.</param>
        public CurentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            UserName = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            Role = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
            Claims = httpContextAccessor.HttpContext?.User?.Claims.AsEnumerable().Select(item => new KeyValuePair<string, string>(item.Type, item.Value)).ToList();
        }

        /// <summary>
        /// Gets the ID of the current user.
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// Gets the name of the current user.
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// Gets the claims associated with the current user.
        /// </summary>
        public List<KeyValuePair<string, string>> Claims { get; set; }

        /// <summary>
        /// Gets the role of the current user.
        /// </summary>
        public string Role { get; }
    }
}
