using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerUI;

namespace Framework.WebApi.Utils;

public static class SwaggerExtensions
{
    /// <summary>
    /// Configures Swagger.
    /// Controllers found by IApiVersionDescriptionProvider will be added to Swagger.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="projectInfo">Project title, description, etc.</param>
    /// <param name="xmlCommentsPaths">collection of paths to files with xml comments which should be included to Swagger.</param>
    /// <param name="useJwtBearerAuthentication">whether JWT Authentication is being used and should be included to Swagger.</param>
    public static IServiceCollection AddSwaggerBss(
            this IServiceCollection services,
            OpenApiInfo projectInfo,
            IEnumerable<string> xmlCommentsPaths = null,
            bool useJwtBearerAuthentication = false)
    {
        services
                .AddSwaggerGen(
                               z =>
                               {
                                   if (xmlCommentsPaths != null)
                                   {
                                       foreach (var xmlCommentsPath in xmlCommentsPaths)
                                       {
                                           z.IncludeXmlComments(xmlCommentsPath);
                                       }
                                   }

                                   if (useJwtBearerAuthentication)
                                   {
                                       z.AddSecurityDefinition(
                                                               JwtBearerDefaults.AuthenticationScheme,
                                                               new OpenApiSecurityScheme
                                                               {
                                                                       Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                                                                       Name = "Authorization",
                                                                       In = ParameterLocation.Header,
                                                                       Type = SecuritySchemeType.ApiKey,
                                                                       Scheme = "Bearer"
                                                               });

                                       z.AddSecurityRequirement(
                                                                new OpenApiSecurityRequirement
                                                                {
                                                                        {
                                                                                new OpenApiSecurityScheme
                                                                                {
                                                                                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                                                                                },
                                                                                new List<string>()
                                                                        }
                                                                });
                                   }

                                   z.IgnoreObsoleteActions();
                                   z.IgnoreObsoleteProperties();
                                   z.CustomSchemaIds(x => x.FullName);

                                   using (var serviceProvider = services.BuildServiceProvider())
                                   {
                                       var provider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();

                                       foreach (var description in provider.ApiVersionDescriptions)
                                       {
                                           z.SwaggerDoc(description.GroupName, projectInfo);
                                       }
                                   }
                               })
                .AddSwaggerGenNewtonsoftSupport();

        return services;
    }

    /// <summary>
    /// Turns on Swagger with /index.html as UI endpoint.
    /// </summary>
    public static IApplicationBuilder UseSwaggerBss(
            this IApplicationBuilder app,
            IApiVersionDescriptionProvider versionProvider,
            Action<SwaggerUIOptions> setupAction = null) =>
            app
                    .UseSwagger()
                    .UseSwaggerUI(
                                  z =>
                                  {
                                      foreach (var description in versionProvider.ApiVersionDescriptions)
                                      {
                                          z.SwaggerEndpoint(
                                                            $"./{description.GroupName}/swagger.json",
                                                            description.GroupName.ToUpperInvariant());
                                      }

                                      z.RoutePrefix = "swagger";
                                      z.ConfigObject.DocExpansion = DocExpansion.None;

                                      setupAction?.Invoke(z);
                                  });

    /// <summary>
    /// Configures versioned API explorer for url path segment versioning.
    /// </summary>
    public static IServiceCollection AddApiVersion(this IServiceCollection services) =>
            services
                    .AddApiVersioning(
                                      z =>
                                      {
                                          z.ReportApiVersions = true;
                                          z.UseApiBehavior = false;
                                          z.AssumeDefaultVersionWhenUnspecified = true;
                                          z.DefaultApiVersion = new ApiVersion(1, 0);
                                      })
                    .AddVersionedApiExplorer(
                                             z =>
                                             {
                                                 z.GroupNameFormat = "'v'VVV";
                                                 z.SubstituteApiVersionInUrl = true;
                                             });
}
