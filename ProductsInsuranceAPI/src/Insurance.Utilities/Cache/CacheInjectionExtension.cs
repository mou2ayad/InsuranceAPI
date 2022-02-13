using System;
using System.Collections.Generic;
using Enyim.Caching.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Utilities.Cache
{
    public static class CacheInjectionExtension
    {
        public static IServiceCollection AddCacheService(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("IsLocalEnv"))
                services.AddSingleton<ICache, InMemoryCache>();
            else
                services.AddDistributedCacheCache(configuration);

            return services;

        }
        private static void AddDistributedCacheCache(this IServiceCollection container, IConfiguration configuration)
        {
            container.AddTransient<ICache, DistributedCache>();
            container.AddEnyimMemcached(o => o.Servers = new List<Server>
            {
                new Server
                {
                    Address = configuration["DistributedCache:Address"],
                    Port = Convert.ToInt32(configuration["DistributedCache:Port"])
                }
            });
        }
    }

}
