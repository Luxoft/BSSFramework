using System.Linq.Expressions;

namespace Framework.SecuritySystem.UserSource;

public record UserPathInfo<TUser>(
    Expression<Func<TUser, Guid>> IdPath,
    Expression<Func<TUser, string>> NamePath,
    Expression<Func<TUser, bool>> Filter) : IUserPathInfo
{
    public Type UserDomainObjectType { get; } = typeof(TUser);
};
