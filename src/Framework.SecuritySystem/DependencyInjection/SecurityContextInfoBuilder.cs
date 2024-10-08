﻿using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.DependencyInjection;

public class SecurityContextInfoBuilder : ISecurityContextInfoBuilder
{
    private readonly List<Action<IServiceCollection>> registerActions = new();

    public ISecurityContextInfoBuilder Add<TSecurityContext>(
        Guid id,
        string? customName = null,
        Func<TSecurityContext, string>? customDisplayFunc = null)
        where TSecurityContext : ISecurityContext
    {
        this.registerActions.Add(
            services =>
            {
                var securityContextInfo = new SecurityContextInfo<TSecurityContext>(
                    id,
                    customName ?? typeof(TSecurityContext).Name);

                services.AddSingleton(securityContextInfo);
                services.AddSingleton<SecurityContextInfo>(securityContextInfo);

                services.AddSingleton<ISecurityContextDisplayService<TSecurityContext>>(
                    new SecurityContextDisplayService<TSecurityContext>(
                        customDisplayFunc ?? (securityContext => securityContext.ToString())));
            });

        return this;
    }

    public void Register(IServiceCollection services)
    {
        this.registerActions.ForEach(action => action(services));
    }
}
