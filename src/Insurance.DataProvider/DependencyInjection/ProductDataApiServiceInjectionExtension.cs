using System;
using Insurance.DataProvider.Configuration;
using Insurance.DataProvider.Contract;
using Insurance.DataProvider.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.DataProvider.DependencyInjection
{
    public static class ProductDataApiServiceInjectionExtension
    {
        public static IServiceCollection AddProductDataApiClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<ProductApiConfig>(configuration.GetSection("ProductApi"));
            services.AddHttpClient<IProductDataApiClient, ProductDataApiClient>(client =>
            {
                client.BaseAddress = new Uri(configuration.GetValue<string>("ProductApi:BaseUrl"));
            });
            if (configuration.GetValue<bool>("ProductApi:EnableCaching"))
                services.Decorate<IProductDataApiClient, ProductDataApiClientCacheDecorator>();


            return services;
        }
    }
}
