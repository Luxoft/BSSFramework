using Bss.Platform.Api.Documentation.SchemaFilters;

using Microsoft.OpenApi.Models;

namespace SampleSystem.WebApiCore.Extensions;

internal static class SwaggerExtension
{
    private const string AuthorizationScheme = "Bearer";

    /// <summary>
    /// Back compatibility fork of AddPlatformApiDocumentation\
    /// Added CustomSchemaIds
    /// </summary>
    public static IServiceCollection AddSwaggerOld(this IServiceCollection services, IWebHostEnvironment hostEnvironment)
    {
        if (hostEnvironment.IsProduction()) return services;

        return services
               .AddEndpointsApiExplorer()
               .AddSwaggerGen(
                   x =>
                   {
                       x.CustomSchemaIds(t => t.FullName);
                       x.SchemaFilter<XEnumNamesSchemaFilter>();
                       x.SwaggerDoc("api", new OpenApiInfo { Title = "SampleSystem API" });

                       x.AddSecurityDefinition(
                           AuthorizationScheme,
                           new OpenApiSecurityScheme
                           {
                               Name = "Authorization",
                               Description = "Specify token",
                               In = ParameterLocation.Header,
                               Type = SecuritySchemeType.ApiKey,
                               Scheme = AuthorizationScheme
                           });

                       x.AddSecurityRequirement(
                           new OpenApiSecurityRequirement
                           {
                               {
                                   new OpenApiSecurityScheme
                                   {
                                       Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = AuthorizationScheme }
                                   },
                                   new List<string>()
                               }
                           });
                   });
    }
}
