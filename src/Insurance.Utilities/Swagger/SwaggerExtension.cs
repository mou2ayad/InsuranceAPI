using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Insurance.Utilities.Swagger
{
    public static class SwaggerExtension
    {
        public static readonly string CommentsDocPath = Path.Combine(AppContext.BaseDirectory, $"InsuranceApi.xml");

        public static IServiceCollection AddSwaggerService(this IServiceCollection services, string apiTitle, IConfiguration configuration, Action<SwaggerGenOptions> setupAction = null)
        {

            OpenApiInfo swaggerDoc = new OpenApiInfo()
            {
                Title = apiTitle,
                Version = configuration.GetValue<string>("Swagger:Version"),
                Description = configuration.GetValue<string>("Swagger:Description")
            };
            if (configuration.GetSection("Swagger:Contact").Exists())
            {
                Uri.TryCreate(configuration.GetValue<string>("Swagger:Contact:Url"), UriKind.Absolute, out var uri);
                swaggerDoc.Contact = new OpenApiContact()
                {
                    Name = configuration.GetValue<string>("Swagger:Contact:Name"),
                    Email = configuration.GetValue<string>("Swagger:Contact:Email"),
                    Url = uri
                };
            }
            var jwtAuth = configuration.GetValue<bool>("Swagger:JWTAuthentication");
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(swaggerDoc.Version, swaggerDoc);
                c.EnableAnnotations();
                c.TagActionsBy(d => new List<string> { d.GroupName });
                c.IncludeXmlComments(CommentsDocPath);
                setupAction?.Invoke(c);

                if (jwtAuth)
                {
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description =
                            "JWT Authorization header using the Bearer scheme",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer",

                    });
                    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {

                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,
                            },
                            new List<string>()
                        }

                    });
                }
            });
            return services;
        }
    }
}
