using System.Linq.Expressions;

namespace Framework.SecuritySystem.Builders.MaterializedBuilder;

public abstract class ByIdentsFilterBuilder<TDomainObject, TSecurityContext>(SecurityContextRestriction<TSecurityContext>? securityContextRestriction) : SecurityFilterBuilder<TDomainObject>
    where TSecurityContext : class, ISecurityContext
{
    public sealed override Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(Dictionary<Type, IEnumerable<Guid>> permission)
    {
        var allowGrandAccess = securityContextRestriction?.Required != true;

        if (permission.TryGetValue(typeof(TSecurityContext), out var securityIdents))
        {
            return this.GetSecurityFilterExpression(securityIdents);
        }
        else
        {
            return _ => allowGrandAccess;
        }
    }

    protected abstract Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(IEnumerable<Guid> securityIdents);
}
