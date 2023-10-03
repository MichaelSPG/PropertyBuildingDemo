using System.Text.Json;
using StackExchange.Redis;
using System;
using PropertyBuildingDemo.Application.IServices;
using Microsoft.Extensions.Options;
using PropertyBuildingDemo.Application.Config;

namespace PropertyBuildingDemo.Infrastructure.Caching
{
    public class CacheService : ICacheService
    {
        private readonly IOptions<ApplicationConfig> _appConfig;
        private IDatabase _db;
        public CacheService(IOptions<ApplicationConfig> appConfig)
        {
            _appConfig = appConfig;
            ConfigureRedis();
        }
        private void ConfigureRedis()
        {
            RedisConnectionHelper.AppConfigOptions = _appConfig;
            _db = RedisConnectionHelper.Connection.GetDatabase();
        }
        public async Task<T> GetDataAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            return default;
        }
        public async Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = await _db.StringSetAsync(key, JsonSerializer.Serialize(value), expiryTime);
            return isSet;
        }
        public async Task<object> RemoveDataAsync(string key)
        {
            bool isKeyExist = await _db.KeyExistsAsync(key);
            if (isKeyExist == true)
            {
                return _db.KeyDelete(key);
            }
            return false;
        }
    }
}
