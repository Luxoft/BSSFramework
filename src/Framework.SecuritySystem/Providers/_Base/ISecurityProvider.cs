using System;
using System.Linq;

using Framework.Core;

namespace Framework.SecuritySystem;

/// <summary>
/// Провайдер доступа к элементам
/// </summary>
/// <typeparam name="TDomainObject"></typeparam>
public interface ISecurityProvider<TDomainObject>
{
    IQueryable<TDomainObject> InjectFilter(IQueryable<TDomainObject> queryable);

    bool HasAccess(TDomainObject domainObject);

    Exception GetAccessDeniedException(TDomainObject domainObject, Func<string, string> formatMessageFunc = null);

    UnboundedList<string> GetAccessors(TDomainObject domainObject);
}
