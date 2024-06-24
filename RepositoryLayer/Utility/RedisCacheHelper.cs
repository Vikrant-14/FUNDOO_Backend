using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RepositoryLayer.Utility
{
    public class RedisCacheHelper
    {
        public static T GetFromCache<T>(string cacheKey, IDistributedCache _cache) where T : class
        {
            var cachedData = _cache.Get(cacheKey);
            if (cachedData != null)
            {
                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                return JsonSerializer.Deserialize<T>(cachedDataString);
            }
            return null;
        }

        public static void SetToCache<T>(string cacheKey, IDistributedCache _cache, T data, int absoluteExpirationMinutes = 2, int slidingExpirationMinutes = 1)
        {
            var cachedDataString = JsonSerializer.Serialize(data);
            var newDataToCache = Encoding.UTF8.GetBytes(cachedDataString);

            var options = new DistributedCacheEntryOptions()
                             .SetAbsoluteExpiration(DateTime.Now.AddMinutes(absoluteExpirationMinutes))
                             .SetSlidingExpiration(TimeSpan.FromMinutes(slidingExpirationMinutes));

            _cache.Set(cacheKey, newDataToCache, options);
        }
    }
}
