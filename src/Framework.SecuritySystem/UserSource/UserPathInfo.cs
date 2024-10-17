using System.Linq.Expressions;

using Framework.Core;

namespace Framework.SecuritySystem.UserSource;

public record UserPathInfo<TUser>(
    Expression<Func<TUser, Guid>> IdPath,
    Expression<Func<TUser, string>> NamePath,
    Expression<Func<TUser, bool>> Filter) : IUserPathInfo
{
    public Type UserDomainObjectType { get; } = typeof(TUser);

    public Expression<Func<TUser, User>> ToDefaultUserExpr { get; } =

        ExpressionHelper.Create((TUser user) => new User(IdPath.Eval(user), NamePath.Eval(user))).InlineEval();
}
