using Framework.Core;
using Framework.Persistent;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ApplicationCore.ExternalSource;

public class SecurityEntitySource : ISecurityEntitySource
{
    private readonly IServiceProvider serviceProvider;

    private readonly ISecurityContextSource securityContextSource;

    private readonly IDictionaryCache<Type, ITypedSecurityEntitySource> typedCache;


    public SecurityEntitySource(IServiceProvider serviceProvider, ISecurityContextSource securityContextSource)
    {
        this.serviceProvider = serviceProvider;
        this.securityContextSource = securityContextSource;

        this.typedCache = new DictionaryCache<Type, ITypedSecurityEntitySource>(this.GetTypedInternal);
    }

    public ITypedSecurityEntitySource GetTyped(Guid securityContextTypeId)
    {
        return this.GetTyped(this.securityContextSource.GetSecurityContextInfo(securityContextTypeId).Type);
    }

    public ITypedSecurityEntitySource GetTyped(Type securityContextType)
    {
        if (securityContextType == null) throw new ArgumentNullException(nameof(securityContextType));

        return this.typedCache[securityContextType];
    }

    private ITypedSecurityEntitySource GetTypedInternal(Type securityContextType)
    {
        if (securityContextType == null) throw new ArgumentNullException(nameof(securityContextType));

        var authorizationTypedExternalSourceType = securityContextType.IsHierarchical()
                                                       ? typeof(HierarchicalTypedSecurityEntitySource<>)
                                                       : typeof(PlainTypedSecurityEntitySource<>);

        var authorizationTypedExternalSourceImplType = authorizationTypedExternalSourceType.MakeGenericType(securityContextType);

        var result = (ITypedSecurityEntitySource)
            ActivatorUtilities.CreateInstance(this.serviceProvider, authorizationTypedExternalSourceImplType);

        return result.WithCache();
    }
}
