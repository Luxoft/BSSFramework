using System.Linq.Expressions;

namespace Framework.SecuritySystem;

public record DependencyDomainSecurityServicePath<TDomainObject, TBaseDomainObject>(
    Expression<Func<TDomainObject, TBaseDomainObject>> Selector);
