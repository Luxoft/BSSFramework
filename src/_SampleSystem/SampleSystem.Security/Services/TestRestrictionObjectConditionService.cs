using System.Linq.Expressions;

using Framework.Core;
using Framework.SecuritySystem;

using SampleSystem.Domain;

namespace SampleSystem.Security.Services;

public class TestRestrictionObjectConditionFactory<TDomainObject>(
    IRelativeDomainPathInfo<TDomainObject, TestRestrictionObject> relativeDomainPathInfo)
    : IFactory<Expression<Func<TDomainObject, bool>>>
{
    public Expression<Func<TDomainObject, bool>> Create() => relativeDomainPathInfo.Path.Select(obj => obj.RestrictionHandler);
}
