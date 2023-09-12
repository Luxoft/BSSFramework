using System.Linq.Expressions;

using Framework.HierarchicalExpand;

namespace Framework.SecuritySystem.DiTests;

public class ExampleAuthorizationSystem : AuthorizationSystem<Guid>
{
    public ExampleAuthorizationSystem(IPrincipalPermissionSource<Guid> principalPermissionSource, IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory)
        : base(principalPermissionSource, hierarchicalObjectExpanderFactory)
    {
    }

    public override Guid ResolveSecurityTypeId(Type type) => throw new NotImplementedException();

    public override bool IsAdmin() => throw new NotImplementedException();

    public override bool HasAccess(NonContextSecurityOperation securityOperation) => throw new NotImplementedException();

    public override void CheckAccess(NonContextSecurityOperation securityOperation) => throw new NotImplementedException();

    public override IEnumerable<string> GetAccessors(NonContextSecurityOperation securityOperation, Expression<Func<IPrincipal<Guid>, bool>> principalFilter) => throw new NotImplementedException();
}
