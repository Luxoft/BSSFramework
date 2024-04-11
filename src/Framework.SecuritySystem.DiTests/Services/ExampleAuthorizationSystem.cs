using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.DiTests;

public class ExampleAuthorizationSystem : IAuthorizationSystem<Guid>
{
    private readonly IPrincipalPermissionSource<Guid> principalPermissionSource;

    private readonly IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory;

    public string CurrentPrincipalName => throw new NotImplementedException();

    public ExampleAuthorizationSystem(
        IPrincipalPermissionSource<Guid> principalPermissionSource,
        IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory)
    {
        this.principalPermissionSource = principalPermissionSource;
        this.hierarchicalObjectExpanderFactory = hierarchicalObjectExpanderFactory;
    }

    public bool IsAdmin() => throw new NotImplementedException();

    public bool HasAccess(SecurityRule.DomainObjectSecurityRule securityRule) => throw new NotImplementedException();

    public void CheckAccess(SecurityRule.DomainObjectSecurityRule securityRule) => throw new NotImplementedException();

    public IEnumerable<string> GetNonContextAccessors(
        SecurityRule.DomainObjectSecurityRule securityRule,
        Expression<Func<IPrincipal<Guid>, bool>> principalFilter) => throw new NotImplementedException();

    public List<Dictionary<Type, IEnumerable<Guid>>> GetPermissions(
        SecurityRule.DomainObjectSecurityRule securityRule,
        IEnumerable<Type> securityTypes)
    {
        return this.principalPermissionSource.GetPermissions()
                   .ToList(permission => this.TryExpandPermission(permission, securityRule.ExpandType));
    }

    public IQueryable<IPermission<Guid>> GetPermissionQuery(
        SecurityRule.DomainObjectSecurityRule securityRule)
    {
        return this.principalPermissionSource.GetPermissionQuery(securityRule);
    }

    private Dictionary<Type, IEnumerable<Guid>> TryExpandPermission(
        Dictionary<Type, List<Guid>> permission,
        HierarchicalExpandType expandType)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        return permission.ToDictionary(
            pair => pair.Key,
            pair => this.hierarchicalObjectExpanderFactory.Create(pair.Key).Expand(pair.Value, expandType));
    }
}
