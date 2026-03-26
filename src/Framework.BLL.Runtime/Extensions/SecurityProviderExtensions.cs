using System.Linq.Expressions;
using Framework.BLL.Providers;
using Framework.DomainDriven.Tracking;

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
        return new FixedPropertiesSecurityProvider<TBLLContext, TDomainObject>(context, securityProvider, allowedPropertiesForChangingExpressions);
    }
}
