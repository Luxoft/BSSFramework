using System.Linq.Expressions;

using Framework.BLL.AccessDeniedExceptionService;
using Framework.BLL.Providers;

using SecuritySystem.Providers;

namespace Framework.BLL.Extensions;

public static class SecurityProviderExtensions
{
    public static ISecurityProvider<TDomainObject> WithFixedProperties<TBLLContext, TDomainObject>(
        this ISecurityProvider<TDomainObject> securityProvider,
        TBLLContext context,
        params Expression<Func<TDomainObject, object>>[] allowedPropertiesForChangingExpressions)

        where TBLLContext : class, IAccessDeniedExceptionServiceContainer, ITrackingServiceContainer<TDomainObject>
        where TDomainObject : class
    {
        if (securityProvider == null) throw new ArgumentNullException(nameof(securityProvider));
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (allowedPropertiesForChangingExpressions == null) throw new ArgumentNullException(nameof(allowedPropertiesForChangingExpressions));

        return new FixedPropertiesSecurityProvider<TBLLContext, TDomainObject>(context, securityProvider, allowedPropertiesForChangingExpressions);
    }
}
