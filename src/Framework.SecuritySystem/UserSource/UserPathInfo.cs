using System.Linq.Expressions;

namespace Framework.SecuritySystem.UserSource;

public record UserPathInfo<TUserDomainObject>(
    Expression<Func<TUserDomainObject, Guid>> IdPath,
    Expression<Func<TUserDomainObject, string>> NamePath,
    Expression<Func<TUserDomainObject, bool>> Filter) : IUserPathInfo
{
    public Type UserDomainObjectType { get; } = typeof(TUserDomainObject);
};
