using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Insurance.Utilities.Cache
{
    public class InMemoryCache :ICache
    {
        private readonly IMemoryCache _memoryCache;

        public InMemoryCache(IMemoryCache memoryCache) => _memoryCache = memoryCache;

        public Task Set<T>(string key, T value, TimeSpan expirationTime)
        {
            _memoryCache.Set(key, value, expirationTime);
            return Task.CompletedTask;
        }

        public Task<T> Get<T>(string key) => Task.FromResult(_memoryCache.Get<T>(key));
    }
}
