using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.DiTests;

public class ExampleAuthorizationSystem(
    IPrincipalPermissionSource<Guid> principalPermissionSource,
    IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory)
    : IAuthorizationSystem<Guid>
{
    public string CurrentPrincipalName => throw new NotImplementedException();

    public bool HasAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule) => throw new NotImplementedException();

    public void CheckAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule) => throw new NotImplementedException();

    public IEnumerable<string> GetNonContextAccessors(
        DomainSecurityRule.RoleBaseSecurityRule securityRule,
        Expression<Func<IPermission<Guid>, bool>> permissionFilter) => throw new NotImplementedException();

    public List<Dictionary<Type, IEnumerable<Guid>>> GetPermissions(
        DomainSecurityRule.RoleBaseSecurityRule securityRule,
        IEnumerable<Type> securityTypes)
    {
        return principalPermissionSource.GetPermissions()
                   .ToList(permission => this.TryExpandPermission(permission, securityRule.SafeExpandType));
    }

    public IQueryable<IPermission<Guid>> GetPermissionQuery(
        DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        return principalPermissionSource.GetPermissionQuery(securityRule);
    }

    private Dictionary<Type, IEnumerable<Guid>> TryExpandPermission(
        Dictionary<Type, List<Guid>> permission,
        HierarchicalExpandType expandType)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        return permission.ToDictionary(
            pair => pair.Key,
            pair => hierarchicalObjectExpanderFactory.Create(pair.Key).Expand(pair.Value, expandType));
    }
}
