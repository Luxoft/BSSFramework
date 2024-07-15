using Framework.Persistent;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.DependencyInjection;

public class SecurityContextInfoBuilder<TIdent> : ISecurityContextInfoBuilder<TIdent>
{
    private readonly List<Action<IServiceCollection>> registerActions = new();

    public ISecurityContextInfoBuilder<TIdent> Add<TSecurityContext>(
        TIdent ident,
        string? customName = null,
        Func<TSecurityContext, string>? customDisplayFunc = null)
        where TSecurityContext : ISecurityContext, IIdentityObject<TIdent>
    {
        this.registerActions.Add(
            services =>
            {
                var securityContextInfo = new SecurityContextInfo<TSecurityContext, TIdent>(
                    ident,
                    customName ?? typeof(TSecurityContext).Name);

                services.AddSingleton(securityContextInfo);
                services.AddSingleton<ISecurityContextInfo>(securityContextInfo);
                services.AddSingleton<ISecurityContextInfo<TIdent>>(securityContextInfo);

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
