using System.Linq.Expressions;

namespace Framework.Authorization.SecuritySystem;

public record UserPathInfo<TDomainObject>(
    Expression<Func<TDomainObject, Guid>> IdPath,
    Expression<Func<TDomainObject, string>> NamePath,
    Expression<Func<TDomainObject, bool>> Filter);
