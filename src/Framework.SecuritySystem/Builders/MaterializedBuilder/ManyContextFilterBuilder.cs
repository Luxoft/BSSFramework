using System.Linq.Expressions;

using Framework.Core;

namespace Framework.SecuritySystem.Builders.MaterializedBuilder;

public class ManyContextFilterBuilder<TDomainObject, TSecurityContext>(
    SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> securityPath)
    : ByIdentsFilterBuilder<TDomainObject, TSecurityContext>
    where TSecurityContext : class, ISecurityContext
{
    protected override Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(IEnumerable<Guid> securityIdents)
    {
        switch (securityPath.Mode)
        {
            case ManySecurityPathMode.AnyStrictly:
            {
                if (securityPath.SecurityPathQ != null)
                {
                    return from securityObjects in securityPath.SecurityPathQ

                           select securityObjects.Any(item => securityIdents.Contains(item.Id));
                }
                else
                {
                    return from securityObjects in securityPath.SecurityPath

                           select securityObjects.Any(item => securityIdents.Contains(item.Id));
                }
            }

            case ManySecurityPathMode.Any:
            {
                if (securityPath.SecurityPathQ != null)
                {
                    return from securityObjects in securityPath.SecurityPathQ

                           select !securityObjects.Any()
                                  || securityObjects.Any(item => securityIdents.Contains(item.Id));
                }
                else
                {
                    return from securityObjects in securityPath.SecurityPath

                           select !securityObjects.Any()
                                  || securityObjects.Any(item => securityIdents.Contains(item.Id));
                }
            }

            default:

                throw new ArgumentOutOfRangeException(nameof(securityPath));
        }
    }
}
