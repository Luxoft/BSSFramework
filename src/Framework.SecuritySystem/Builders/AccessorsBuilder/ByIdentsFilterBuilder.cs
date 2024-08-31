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

        var getIdents = permissionSystem.GetPermissionRestrictions(typeof(TSecurityContext));

        var fullAccessFilter = getIdents.Select(ident => !ident.Any());

        if (securityObjects.Any())
        {
            var securityIdents = hierarchicalObjectExpanderFactory
                                 .Create(typeof(TSecurityContext))
                                 .Expand(securityObjects.Select(securityObject => securityObject.Id), expandType.Reverse());

            return fullAccessFilter.BuildOr(
                getIdents.Select(restrictionIdents => restrictionIdents.Any(restrictionIdent => securityIdents.Contains(restrictionIdent))));
        }
        else
        {
            return fullAccessFilter;
        }
    }

    protected abstract IEnumerable<TSecurityContext> GetSecurityObjects(TDomainObject domainObject);
}
