using System.Linq.Expressions;

namespace Framework.Authorization.SecuritySystem;

public record PrincipalIdentitySourcePathInfo<TDomainObject>(Expression<Func<TDomainObject, string>> Path);
