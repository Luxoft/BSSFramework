using Framework.Core;
using Framework.Authorization.Domain;
using Framework.Persistent;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem.ExternalSource;

public class AuthorizationExternalSource : IAuthorizationExternalSource
{
    private readonly IServiceProvider serviceProvider;

    private readonly ISecurityContextInfoService securityContextInfoService;

    private readonly IDictionaryCache<SecurityContextType, IAuthorizationTypedExternalSourceBase> typedCache;


    public AuthorizationExternalSource(IServiceProvider serviceProvider, ISecurityContextInfoService securityContextInfoService)
    {
        this.serviceProvider = serviceProvider;
        this.securityContextInfoService = securityContextInfoService;

        this.typedCache = new DictionaryCache<SecurityContextType, IAuthorizationTypedExternalSourceBase>(entityType => this.GetTypedInternal(entityType, true));
    }

    public IAuthorizationTypedExternalSourceBase GetTyped(SecurityContextType entityType, bool withCache = true)
    {
        if (entityType == null) throw new ArgumentNullException(nameof(entityType));

        return withCache ? this.typedCache[entityType] : this.GetTypedInternal(entityType, false);
    }

    private IAuthorizationTypedExternalSourceBase GetTypedInternal(SecurityContextType entityType, bool withCache)
    {
        if (entityType == null) throw new ArgumentNullException(nameof(entityType));

        var securityContextInfo = this.securityContextInfoService.GetSecurityContextInfo(entityType.Name);

        var authorizationTypedExternalSourceType = securityContextInfo.Type.IsHierarchical()
                                                           ? typeof(HierarchicalAuthorizationTypedExternalSource<>)
                                                           : typeof(PlainAuthorizationTypedExternalSource<>);

        var authorizationTypedExternalSourceImplType = authorizationTypedExternalSourceType.MakeGenericType(securityContextInfo.Type);

        var result = (IAuthorizationTypedExternalSourceBase)
            ActivatorUtilities.CreateInstance(this.serviceProvider, authorizationTypedExternalSourceImplType);

        return withCache ? result.WithCache() : result;
    }
}
