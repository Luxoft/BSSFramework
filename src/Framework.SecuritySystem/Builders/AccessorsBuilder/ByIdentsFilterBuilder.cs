using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.Builders.AccessorsBuilder;

public abstract class ByIdentsFilterBuilder<TPermission, TDomainObject, TSecurityContext>(
    IPermissionSystem<TPermission> permissionSystem,
    IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
    SecurityContextRestriction<TSecurityContext>? securityContextRestriction) : AccessorsFilterBuilder<TPermission, TDomainObject>
    where TSecurityContext : class, ISecurityContext
{
    protected abstract bool AllowEmpty { get; }

    public override Expression<Func<TPermission, bool>> GetAccessorsFilter(
        TDomainObject domainObject,
        HierarchicalExpandType expandType)
    {
        var securityObjects = this.GetSecurityObjects(domainObject).ToArray();

        var allowGrandAccess = securityContextRestriction?.Required != true;

        var grandAccessExpr = allowGrandAccess
                                  ? permissionSystem.GetGrandAccessExpr<TSecurityContext>()
                                  : _ => false;

        if (securityObjects.Any())
        {
            var securityIdents = hierarchicalObjectExpanderFactory
                                 .Create(typeof(TSecurityContext))
                                 .Expand(securityObjects.Select(securityObject => securityObject.Id), expandType.Reverse());

            return grandAccessExpr.BuildOr(permissionSystem.GetContainsIdentsExpr(securityIdents, securityContextRestriction?.Filter));
        }
        else
        {
            if (this.AllowEmpty)
            {
                return _ => true;
            }
            else
            {
                return grandAccessExpr;
            }
        }
    }

    protected abstract IEnumerable<TSecurityContext> GetSecurityObjects(TDomainObject domainObject);
}
