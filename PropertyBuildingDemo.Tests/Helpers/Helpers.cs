using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Tests.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PropertyBuildingDemo.Tests.Helpers.TestConstants;

namespace PropertyBuildingDemo.Tests.Helpers
{
    public class Helpers
    {
        public class TestEndpoint
        {
            private static readonly Dictionary<Type, IEndpointUrl> Endpoints = new();

            // Private constructor to prevent external instantiation.
            static TestEndpoint()
            {
                Endpoints[typeof(OwnerDto)] = new OwnerEndpoint();
                Endpoints[typeof(PropertyImageDto)] = new PropertyImageEndpoint();
                Endpoints[typeof(PropertyTraceDto)] = new PropertyTraceEndpoint();
                // Add more registrations for other entity types as needed.
            }

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
}
