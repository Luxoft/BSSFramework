using Framework.Core;
using Framework.Authorization.Domain;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem.ExternalSource;

public class AuthorizationExternalSource : IAuthorizationExternalSource
{
    private readonly IServiceProvider serviceProvider;

    private readonly ISecurityContextInfoService securityContextInfoService;

    private readonly IDictionaryCache<SecurityContextType, IAuthorizationTypedExternalSource> typedCache;


    public AuthorizationExternalSource(IServiceProvider serviceProvider, ISecurityContextInfoService securityContextInfoService)
    {
        this.serviceProvider = serviceProvider;
        this.securityContextInfoService = securityContextInfoService;

        this.typedCache = new DictionaryCache<SecurityContextType, IAuthorizationTypedExternalSource>(entityType => this.GetTypedInternal(entityType, true));
    }

    public IAuthorizationTypedExternalSource GetTyped(SecurityContextType entityType, bool withCache = true)
    {
        if (entityType == null) throw new ArgumentNullException(nameof(entityType));

        return withCache ? this.typedCache[entityType] : this.GetTypedInternal(entityType, false);
    }

    private IAuthorizationTypedExternalSource GetTypedInternal(SecurityContextType entityType, bool withCache)
    {
        if (entityType == null) throw new ArgumentNullException(nameof(entityType));

        var domainType = this.securityContextInfoService.GetSecurityContextInfo(entityType.Name).Type;

        var authorizationTypedExternalSourceType = entityType.Expandable
                                                           ? typeof(HierarchicalAuthorizationTypedExternalSource<>)
                                                           : typeof(PlainAuthorizationTypedExternalSource<>);

        var authorizationTypedExternalSourceImplType = authorizationTypedExternalSourceType.MakeGenericType(domainType);

        var result = (IAuthorizationTypedExternalSource)
            ActivatorUtilities.CreateInstance(this.serviceProvider, authorizationTypedExternalSourceImplType);

        return withCache ? result.WithCache() : result;
    }
}
