using System.Linq.Expressions;

using Framework.DomainDriven.Tracking;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security;

public static class SecurityProviderExtensions
{
    public static ISecurityProvider<TDomainObject> WithFixedProperties<TBLLContext, TDomainObject>(
        this ISecurityProvider<TDomainObject> securityProvider,
        TBLLContext context,
        params Expression<Func<TDomainObject, object>>[] allowedPropertiesForChangingExpressions)

        where TBLLContext : class, IAccessDeniedExceptionServiceContainer<TDomainObject>, ITrackingServiceContainer<TDomainObject>
        where TDomainObject : class
    {
        if (securityProvider == null) throw new ArgumentNullException(nameof(securityProvider));
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (allowedPropertiesForChangingExpressions == null) throw new ArgumentNullException(nameof(allowedPropertiesForChangingExpressions));

        return new FixedPropertiesSecurityProvider<TBLLContext, TDomainObject>(context, securityProvider, allowedPropertiesForChangingExpressions);
    }
}
