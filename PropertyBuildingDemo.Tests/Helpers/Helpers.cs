using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Tests.Helpers.Config;

namespace PropertyBuildingDemo.Tests.Helpers
{
    /// <summary>
    /// Factory class for getting endpoint URLs based on entity types for testing purposes.
    /// </summary>
    public class TestEndpoint
    {
        private static readonly Dictionary<Type, IEndpointUrl> Endpoints = new();

        /// <summary>
        /// Static constructor to initialize endpoint URLs for supported entity types.
        /// </summary>
        static TestEndpoint()
        {
            Endpoints[typeof(OwnerDto)] = new OwnerEndpoint();
            Endpoints[typeof(PropertyImageDto)] = new PropertyImageEndpoint();
            Endpoints[typeof(PropertyTraceDto)] = new PropertyTraceEndpoint();
            // Add more registrations for other entity types as needed.
        }

        /// <summary>
        /// Gets the endpoint URL for a specific entity type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type for which to get the endpoint URL.</typeparam>
        /// <returns>The endpoint URL for the specified entity type.</returns>
        /// <exception cref="NotSupportedException">Thrown when an endpoint URL for the given entity type is not supported.</exception>
        public static IEndpointUrl GetEndpoint<TEntity>()
        {
            if (Endpoints.TryGetValue(typeof(TEntity), out IEndpointUrl factory))
            {
                return (IEndpointUrl)factory;
            }

            throw new NotSupportedException($"EndpointUrl for {typeof(TEntity)} is not supported.");
        }
    }
}
