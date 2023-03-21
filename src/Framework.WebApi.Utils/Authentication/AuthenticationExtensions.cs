using System;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Framework.WebApi.Utils;

public static class AuthenticationExtensions
{
    /// <summary>
    /// Turns on JWT authentication.
    /// Requires "JwtBearer" configuration section with following properties: Issuer, Audience.
    /// </summary>
    public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtBearerSettings = new JwtBearerSettings();
        configuration.GetSection("JwtBearer").Bind(jwtBearerSettings);

        services
                .AddAuthentication(
                                   z =>
                                   {
                                       z.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                                       z.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                                       z.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                   })
                .AddJwtBearer(
                              z =>
                              {
                                  z.MetadataAddress = $"{jwtBearerSettings.Issuer}/adfs/.well-known/openid-configuration";
                                  z.Validate();
                                  z.TokenValidationParameters = new TokenValidationParameters
                                                                {
                                                                        ValidIssuer = $"http://{new Uri(jwtBearerSettings.Issuer).Host}/adfs/services/trust",
                                                                        ValidAudience = jwtBearerSettings.Audience,
                                                                        ValidateAudience = true,
                                                                        ValidateIssuerSigningKey = true,
                                                                        ValidateLifetime = true,
                                                                        RequireSignedTokens = true,
                                                                        ValidateActor = true
                                                                };
                              });

        return services;
    }
}
