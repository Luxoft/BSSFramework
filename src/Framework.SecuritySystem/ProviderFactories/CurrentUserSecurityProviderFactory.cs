using Framework.Core;
using Framework.SecuritySystem.UserSource;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.ProviderFactories;

public class CurrentUserSecurityProviderFactory<TDomainObject>(
    IServiceProvider serviceProvider)
    : IDefaultSecurityProviderFactory<TDomainObject, DomainSecurityRule.CurrentUserSecurityRule>
{
    public ISecurityProvider<TDomainObject> Create(DomainSecurityRule.CurrentUserSecurityRule securityRule, SecurityPath<TDomainObject>? securityPath)
    {
        var args = new object?[]
            {
                securityRule.RelativePathKey == null
                    ? null
                    : new CurrentUserSecurityProviderRelativeKey(securityRule.RelativePathKey)
            }.Where(arg => arg != null)
             .ToArray(arg => arg!);

        return ActivatorUtilities.CreateInstance<CurrentUserSecurityProvider<TDomainObject>>(serviceProvider, args);
    }
}
