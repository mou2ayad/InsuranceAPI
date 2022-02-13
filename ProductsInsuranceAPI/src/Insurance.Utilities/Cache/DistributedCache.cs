using System;
using System.Threading.Tasks;
using Enyim.Caching;
using Microsoft.Extensions.Logging;

namespace Insurance.Utilities.Cache
{
    public class DistributedCache : ICache
    {
        private readonly IMemcachedClient _cachedClient;
        private readonly ILogger<DistributedCache> _logger;

        public DistributedCache(IMemcachedClient client, ILogger<DistributedCache> logger)
        {
            _cachedClient = client;
            _logger = logger;
        }

        public async Task<T> Get<T>(string key)
        {
            try
            {
                var value = await _cachedClient.GetValueAsync<T>(key);
                if (value == null)
                    _logger.LogInformation("{@name} {@key} is not found in distributed Cache", typeof(T).Name, key);
                return value;
            }
            catch (Exception e)
            {
                _logger.LogError(e, key);
                return await Task.FromResult(default(T));
            }
        }

        public async Task Set<T>(string key, T value, TimeSpan expirationTime)
        {
            if (await _cachedClient.SetAsync(key, value, (int)expirationTime.TotalSeconds))
                _logger.LogInformation("{@name} {@key} is created", typeof(T).Name, key);
            else
                _logger.LogError("failed to create {@name} {@key} in distributed Cache", typeof(T).Name, key);
        }
    }
}