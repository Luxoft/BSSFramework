using System.Linq.Expressions;

using Framework.Core;

namespace Framework.SecuritySystem.Builders.MaterializedBuilder;

public class SingleContextFilterBuilder<TDomainObject, TSecurityContext>(
    SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> securityPath,
    SecurityContextRestriction<TSecurityContext>? securityContextRestriction)
    : ByIdentsFilterBuilder<TDomainObject, TSecurityContext>(securityContextRestriction)
    where TSecurityContext : class, ISecurityContext
{
    protected override Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(IEnumerable<Guid> securityIdents)
    {
        if (securityPath.Required)
        {
            return from securityObject in securityPath.Expression

                   select securityIdents.Contains(securityObject.Id);
        }
        else
        {
            return from securityObject in securityPath.Expression

                   select securityObject == null || securityIdents.Contains(securityObject.Id);
        }
    }
}
