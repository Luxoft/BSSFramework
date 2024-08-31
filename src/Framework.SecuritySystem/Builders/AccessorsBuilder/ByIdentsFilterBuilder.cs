using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.Builders.AccessorsBuilder;

public abstract class ByIdentsFilterBuilder<TPermission, TDomainObject, TSecurityContext>(
    IPermissionSystem<TPermission> permissionSystem,
    IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory) : AccessorsFilterBuilder<TPermission, TDomainObject>
    where TSecurityContext : class, ISecurityContext, IIdentityObject<Guid>
{
    public override Expression<Func<TPermission, bool>> GetAccessorsFilter(
        TDomainObject domainObject,
        HierarchicalExpandType expandType)
    {
        var securityObjects = this.GetSecurityObjects(domainObject).ToArray();

        var grandAccessExpr = permissionSystem.GetGrandAccessExpr<TSecurityContext>();

        if (securityObjects.Any())
        {
            var getIdents = permissionSystem.GetPermissionRestrictionsExpr<TSecurityContext>();

            var securityIdents = hierarchicalObjectExpanderFactory
                                 .Create(typeof(TSecurityContext))
                                 .Expand(securityObjects.Select(securityObject => securityObject.Id), expandType.Reverse());

            return grandAccessExpr.BuildOr(
                getIdents.Select(restrictionIdents => restrictionIdents.Any(restrictionIdent => securityIdents.Contains(restrictionIdent))));
        }
        else
        {
            return grandAccessExpr;
        }
    }

    protected abstract IEnumerable<TSecurityContext> GetSecurityObjects(TDomainObject domainObject);
}
