using System.Linq.Expressions;

namespace Framework.SecuritySystem;

public record DependencyDomainSecurityServicePathInfo<TDomainObject, TBaseDomainObject>(
    Expression<Func<TDomainObject, TBaseDomainObject>> Path);
