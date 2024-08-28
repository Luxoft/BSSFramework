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

    private readonly IDictionaryCache<SecurityContextType, IAuthorizationTypedExternalSource> typedCache;


    public AuthorizationExternalSource(IServiceProvider serviceProvider, ISecurityContextInfoService securityContextInfoService)
    {
        this.serviceProvider = serviceProvider;
        this.securityContextInfoService = securityContextInfoService;

        this.typedCache = new DictionaryCache<SecurityContextType, IAuthorizationTypedExternalSource>(securityContextType => this.GetTypedInternal(securityContextType, true));
    }

    public IAuthorizationTypedExternalSource GetTyped(SecurityContextType securityContextType, bool withCache = true)
    {
        if (securityContextType == null) throw new ArgumentNullException(nameof(securityContextType));

        return withCache ? this.typedCache[securityContextType] : this.GetTypedInternal(securityContextType, false);
    }

    private IAuthorizationTypedExternalSource GetTypedInternal(SecurityContextType securityContextType, bool withCache)
    {
        if (securityContextType == null) throw new ArgumentNullException(nameof(securityContextType));

        var securityContextInfo = this.securityContextInfoService.GetSecurityContextInfo(securityContextType.Id);

        var authorizationTypedExternalSourceType = securityContextInfo.Type.IsHierarchical()
                                                           ? typeof(HierarchicalAuthorizationTypedExternalSource<>)
                                                           : typeof(PlainAuthorizationTypedExternalSource<>);

        var authorizationTypedExternalSourceImplType = authorizationTypedExternalSourceType.MakeGenericType(securityContextInfo.Type);

        var result = (IAuthorizationTypedExternalSource)
            ActivatorUtilities.CreateInstance(this.serviceProvider, authorizationTypedExternalSourceImplType);

        return withCache ? result.WithCache() : result;
    }
}
