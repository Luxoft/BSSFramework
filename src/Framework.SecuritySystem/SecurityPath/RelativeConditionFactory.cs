using System.Linq.Expressions;

using Framework.Core;

namespace Framework.SecuritySystem;

public class RelativeConditionFactory<TDomainObject, TRelativeDomainObject>(
    RelativeConditionInfo<TRelativeDomainObject> conditionInfo,
    IRelativeDomainPathInfo<TDomainObject, TRelativeDomainObject>? relativeDomainPathInfo = null)
    : IFactory<Expression<Func<TDomainObject, bool>>?>
{
    public Expression<Func<TDomainObject, bool>>? Create() => relativeDomainPathInfo?.Path.Select(conditionInfo.Condition);
}

public class RequiredRelativeConditionFactory<TDomainObject, TRelativeDomainObject>(
    RelativeConditionInfo<TRelativeDomainObject> conditionInfo,
    IRelativeDomainPathInfo<TDomainObject, TRelativeDomainObject> relativeDomainPathInfo)
    : IFactory<Expression<Func<TDomainObject, bool>>>
{
    public Expression<Func<TDomainObject, bool>> Create() => relativeDomainPathInfo.Path.Select(conditionInfo.Condition);
}
