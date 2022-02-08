using Insurance.Api.DependencyInjection;
using Insurance.Utilities.Cache;
using Insurance.Utilities.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Insurance.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private readonly string API_NAME = "InsuranceAPI";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHealthChecks();
            services.AddMemoryCache()
                .AddInsuranceService(Configuration)
                .AddCacheService(Configuration)
                .AddSwaggerService(API_NAME, Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerMiddleware(API_NAME, Configuration)
                .UseHttpsRedirection()
                .UseRouting()
                .UseHealthChecks(new Microsoft.AspNetCore.Http.PathString("/healthcheck"));

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
