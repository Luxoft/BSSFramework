using System.Linq.Expressions;

using Framework.Core;
using Framework.Persistent;

namespace Framework.SecuritySystem.Builders.MaterializedBuilder;

public class SingleContextFilterBuilder<TDomainObject, TSecurityContext>(
    SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> securityPath)
    : ByIdentsFilterBuilder<TDomainObject, TSecurityContext>
    where TSecurityContext : class, ISecurityContext, IIdentityObject<Guid>
{
    protected override Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(IEnumerable<Guid> securityIdents)
    {
        switch (securityPath.Mode)
        {
            case SingleSecurityMode.AllowNull:

                return from securityObject in securityPath.SecurityPath

                       select securityObject == null || securityIdents.Contains(securityObject.Id);

            case SingleSecurityMode.Strictly:

                return from securityObject in securityPath.SecurityPath

                       select securityIdents.Contains(securityObject.Id);

            default: throw new ArgumentOutOfRangeException(securityPath.Mode.ToString());
        }
    }
}
