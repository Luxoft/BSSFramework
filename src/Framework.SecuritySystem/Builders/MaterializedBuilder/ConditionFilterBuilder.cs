using System.Linq.Expressions;

namespace Framework.SecuritySystem.Builders.MaterializedBuilder;

public class ConditionFilterBuilder<TDomainObject>(
    SecurityPath<TDomainObject>.ConditionPath securityPath)
    : SecurityFilterBuilder<TDomainObject>
{
    public override Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(Dictionary<Type, IEnumerable<Guid>> _) =>
        securityPath.SecurityFilter;
}
