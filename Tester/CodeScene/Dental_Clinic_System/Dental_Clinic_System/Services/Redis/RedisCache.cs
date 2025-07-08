using Dental_Clinic_System.Models.Data;
using Microsoft.Extensions.Caching.Distributed;

namespace Dental_Clinic_System.Services.Redis
{
    public class RedisCache
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<RedisCache> _logger;

        public RedisCache(IDistributedCache cache, ILogger<RedisCache> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<string> GetCachedDataAsync(string key)
        {
            var cachedData = await _cache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(cachedData))
            {
                _logger.LogInformation("Cache hit for key: {CacheKey}", key);
                return cachedData;
            }

            _logger.LogInformation("Cache miss for key: {CacheKey}", key);
            var data = "This is the data to cache"; // Retrieve data from your database or any other source
            await _cache.SetStringAsync(key, data, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            return data;
        }
    }
}
