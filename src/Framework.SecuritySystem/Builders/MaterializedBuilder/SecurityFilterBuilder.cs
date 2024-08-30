using System.Linq.Expressions;

namespace Framework.SecuritySystem.Builders.MaterializedBuilder;

public abstract class SecurityFilterBuilder<TDomainObject>
{
    public abstract Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(Dictionary<Type, IEnumerable<Guid>> permission);
}
