using System.Linq.Expressions;

using Framework.Core;

namespace Framework.SecuritySystem;

public class RelativeConditionFactory<TDomainObject, TRelativeTargetDomainObject>(
    SecurityPathRestrictionConditionInfo<TRelativeTargetDomainObject> conditionInfo,
    IRelativeDomainPathInfo<TDomainObject, TRelativeTargetDomainObject>? relativeDomainPathInfo = null)
    : IFactory<Expression<Func<TDomainObject, bool>>?>
{
    public Expression<Func<TDomainObject, bool>>? Create() => relativeDomainPathInfo?.Path.Select(conditionInfo.Condition);
}
