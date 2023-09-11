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

    public override bool HasAccess<TSecurityOperationCode>(TSecurityOperationCode securityOperationCode) => throw new NotImplementedException();

    public override void CheckAccess<TSecurityOperationCode>(TSecurityOperationCode securityOperationCode) => throw new NotImplementedException();

    public override IEnumerable<string> GetAccessors<TSecurityOperationCode>(TSecurityOperationCode securityOperationCode, Expression<Func<IPrincipal<Guid>, bool>> principalFilter) => throw new NotImplementedException();
}
