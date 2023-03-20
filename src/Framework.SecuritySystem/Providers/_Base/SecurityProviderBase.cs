using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Framework.Core;

namespace Framework.SecuritySystem;

public abstract class SecurityProviderBase<TDomainObject> : ISecurityProvider<TDomainObject>

        where TDomainObject : class
{
    protected readonly IAccessDeniedExceptionService<TDomainObject> AccessDeniedExceptionService;

    protected SecurityProviderBase([NotNull] IAccessDeniedExceptionService<TDomainObject> accessDeniedExceptionService)
    {
        this.AccessDeniedExceptionService = accessDeniedExceptionService ?? throw new ArgumentNullException(nameof(accessDeniedExceptionService));
    }


    public abstract IQueryable<TDomainObject> InjectFilter([NotNull] IQueryable<TDomainObject> queryable);

    public abstract bool HasAccess([NotNull] TDomainObject domainObject);

    public abstract UnboundedList<string> GetAccessors([NotNull] TDomainObject domainObject);


    public virtual Exception GetAccessDeniedException(TDomainObject domainObject, Func<string, string> formatMessageFunc = null)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        return this.AccessDeniedExceptionService.GetAccessDeniedException(domainObject, null, formatMessageFunc);
    }
}
