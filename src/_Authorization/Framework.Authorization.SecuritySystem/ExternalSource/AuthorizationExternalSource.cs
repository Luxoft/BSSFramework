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

        this.typedCache = new DictionaryCache<SecurityContextType, IAuthorizationTypedExternalSourceBase>(securityContextType => this.GetTypedInternal(securityContextType, true));
    }

    public IAuthorizationTypedExternalSourceBase GetTyped(SecurityContextType securityContextType, bool withCache = true)
    {
        if (securityContextType == null) throw new ArgumentNullException(nameof(securityContextType));

        return withCache ? this.typedCache[securityContextType] : this.GetTypedInternal(securityContextType, false);
    }

    private IAuthorizationTypedExternalSourceBase GetTypedInternal(SecurityContextType securityContextType, bool withCache)
    {
        if (securityContextType == null) throw new ArgumentNullException(nameof(securityContextType));

        var securityContextInfo = this.securityContextInfoService.GetSecurityContextInfo(securityContextType.Name);

        var authorizationTypedExternalSourceType = securityContextInfo.Type.IsHierarchical()
                                                           ? typeof(HierarchicalAuthorizationTypedExternalSource<>)
                                                           : typeof(PlainAuthorizationTypedExternalSource<>);

        var authorizationTypedExternalSourceImplType = authorizationTypedExternalSourceType.MakeGenericType(securityContextInfo.Type);

        var result = (IAuthorizationTypedExternalSourceBase)
            ActivatorUtilities.CreateInstance(this.serviceProvider, authorizationTypedExternalSourceImplType);

        return withCache ? result.WithCache() : result;
    }
}
