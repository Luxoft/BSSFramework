using System.Data;

using DotNetCore.CAP;

using Framework.Cap.Abstractions;
using Framework.Cap.Auth;
using Framework.Cap.Impl;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

using Savorboard.CAP.InMemoryMessageQueue;

namespace Framework.Cap;

public static class DependencyInjections
{
    public const string CapAuthenticationScheme = nameof(CapAuthenticationScheme);

    /// <summary>
    /// Add CAP with authentication. For authentication add this code
    /// <code>
    ///     services.AddAuthentication().AddCapAuth&lt;ISampleSystemBLLContext&gt;();
    /// </code>
    /// </summary>
    public static IServiceCollection AddCapBss(
            this IServiceCollection services,
            string connectionString,
            Action<CapOptions> setupAction = null)
    {
        services
                .AddScoped<ICapTransaction>(
                    serviceProvider =>
                    {
                        var dbTransaction = serviceProvider.GetRequiredService<IDbTransaction>();

                        var capTransaction = ActivatorUtilities.CreateInstance<SqlServerCapTransaction>(serviceProvider);

                        capTransaction.DbTransaction = dbTransaction;

                        return capTransaction;
                    })
                .AddScoped<IIntegrationEventBus, IntegrationEventBus>()
                .AddCap(
                        z =>
                        {
                            z.FailedRetryCount = 2;
                            z.UseSqlServer(
                                           x =>
                                           {
                                               x.ConnectionString = connectionString;
                                               x.Schema = "bus";
                                           });
                            z.UseInMemoryMessageQueue();

                            z.UseDashboard(
                                           x =>
                                           {
                                               x.UseAuth = true;
                                               x.DefaultAuthenticationScheme = CapAuthenticationScheme;
                                           });

                            setupAction?.Invoke(z);
                        });

        return services;
    }

    /// <summary>
    /// Add CAP authentication (User with role Administrator has access only)
    /// </summary>
    public static AuthenticationBuilder AddCapAuth(
            this AuthenticationBuilder builder,
            Action<AuthenticationSchemeOptions> configureOptions = null) =>
            builder.AddScheme<AuthenticationSchemeOptions, CapAuthenticationHandler>(
             CapAuthenticationScheme,
             CapAuthenticationScheme,
             configureOptions);
}
