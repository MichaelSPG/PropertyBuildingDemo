using StackExchange.Redis;
using System;
using Microsoft.Extensions.Options;
using PropertyBuildingDemo.Application.Config;

namespace PropertyBuildingDemo.Infrastructure.Caching
{
    public class RedisConnectionHelper
    {
        public static IOptions<ApplicationConfig> AppConfigOptions;
        static RedisConnectionHelper()
        {
            RedisConnectionHelper.lazyConnection = new Lazy<ConnectionMultiplexer>(() => {
                return ConnectionMultiplexer.Connect(AppConfigOptions.Value.RedisUrl);
            });
        }
        private static Lazy<ConnectionMultiplexer> lazyConnection;
        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}
