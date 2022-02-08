using Insurance.Api.Contracts;
using Insurance.Api.Models;
using Insurance.Api.Services;
using Insurance.DataProvider.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Api.DependencyInjection
{
    public static class InsuranceServicesInjectionExtension
    {
        public static IServiceCollection AddInsuranceService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<InsuranceCostServiceConfig>(configuration.GetSection("InsuranceServiceConfig"));
            services.AddSingleton<IInsuranceCostService, InsuranceCostService>()
                .AddProductDataApiClient(configuration);

            if (configuration.GetValue<bool>("InsuranceServiceConfig:EnableCaching"))
                services.Decorate<IInsuranceCostService, InsuranceCostServiceCacheDecorator>();

            return services;
        }
    }
}
