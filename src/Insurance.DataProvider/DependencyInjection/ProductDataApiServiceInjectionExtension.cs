using System;
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
            services.AddHttpClient<IProductDataApiClient, ProductDataApiClient>(client =>
            {
                client.BaseAddress = new Uri(configuration.GetValue<string>("ProductApi:BaseUrl"));
            });
           
            return services;
        }
    }
}
