using Framework.Core;
using Framework.Persistent;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.ExternalSystem.SecurityContextStorage;

public class SecurityContextStorage : ISecurityContextStorage
{
    private readonly IServiceProvider serviceProvider;

    private readonly ISecurityContextInfoSource securityContextInfoSource;

    private readonly IDictionaryCache<Type, ITypedSecurityContextStorage> typedCache;


    public SecurityContextStorage(IServiceProvider serviceProvider, ISecurityContextInfoSource securityContextInfoSource)
    {
        this.serviceProvider = serviceProvider;
        this.securityContextInfoSource = securityContextInfoSource;

        this.typedCache = new DictionaryCache<Type, ITypedSecurityContextStorage>(this.GetTypedInternal);
    }

    public ITypedSecurityContextStorage GetTyped(Guid securityContextTypeId)
    {
        return this.GetTyped(this.securityContextInfoSource.GetSecurityContextInfo(securityContextTypeId).Type);
    }

    public ITypedSecurityContextStorage GetTyped(Type securityContextType)
    {
        if (securityContextType == null) throw new ArgumentNullException(nameof(securityContextType));

        return this.typedCache[securityContextType];
    }

    private ITypedSecurityContextStorage GetTypedInternal(Type securityContextType)
    {
        if (securityContextType == null) throw new ArgumentNullException(nameof(securityContextType));

        var authorizationTypedExternalSourceType = securityContextType.IsHierarchical()
                                                       ? typeof(HierarchicalTypedSecurityContextStorage<>)
                                                       : typeof(PlainTypedSecurityContextStorage<>);

        var authorizationTypedExternalSourceImplType = authorizationTypedExternalSourceType.MakeGenericType(securityContextType);

        var result = (ITypedSecurityContextStorage)
            ActivatorUtilities.CreateInstance(this.serviceProvider, authorizationTypedExternalSourceImplType);

        return result.WithCache();
    }
}
