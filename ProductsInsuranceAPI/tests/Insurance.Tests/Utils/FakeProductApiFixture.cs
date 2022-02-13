using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Insurance.Tests.Utils
{
    public class FakeProductApiFixture : IDisposable
    {
        private readonly IHost _host;
        public readonly string BaseUrl = "http://localhost:5000/";
        public FakeProductApiFixture()
        {
            _host = new HostBuilder()
                .ConfigureWebHostDefaults(
                    b => b.UseUrls(BaseUrl)
                        .UseStartup<FakeProductApiStartup>()
                )
                .Build();

            _host.Start();
        }

        public void Dispose() => _host.Dispose();
        internal class FakeProductApiStartup
        {
            public void Configure(IApplicationBuilder app)
            {
                app.UseRouting();
                app.UseEndpoints(
                    ep =>
                    {
                        // product endpoint returns product model
                        ep.MapGet(
                            "products/{id:int}",
                            context =>
                            {
                                int productId = int.Parse((string)context.Request.RouteValues["id"]);
                                var product = new
                                {
                                    id = productId,
                                    name = "Test Product",
                                    productTypeId = 1,
                                    salesPrice = 750
                                };
                                return context.Response.WriteAsync(JsonConvert.SerializeObject(product));
                            }
                        );
                        // product endpoint returns 404
                        ep.MapGet(
                            "products/404",
                            context =>
                            {
                                var error = new
                                {
                                    type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                                    title = "Not Found",
                                    status = 404,
                                    traceId = "1234"
                                };
                                context.Response.StatusCode = StatusCodes.Status404NotFound;
                                return context.Response.WriteAsync(JsonConvert.SerializeObject(error));
                            }
                        );
                        // product endpoint returns connection exception BadGateway
                        ep.MapGet("products/502",
                            context =>
                            {
                                context.Response.StatusCode = StatusCodes.Status502BadGateway;
                                return context.Response.CompleteAsync();
                            }
                        );

                        ep.MapGet(
                            "products/999",
                            context =>
                            {
                                var product =
                                    new
                                    {
                                        canBeInsured = true
                                    };
                                return context.Response.WriteAsync(JsonConvert.SerializeObject(product));
                            }
                        );

                        // product type endpoint returns productType model

                        ep.MapGet(
                            "products_type/{id:int}",
                            context =>
                            {
                                var productType =
                                    new
                                    {
                                        id = int.Parse((string)context.Request.RouteValues["id"]),
                                        name = "Test type",
                                        canBeInsured = true
                                    };
                                return context.Response.WriteAsync(JsonConvert.SerializeObject(productType));
                            }
                        );
                    }
                );
            }
        }
    }
  
}