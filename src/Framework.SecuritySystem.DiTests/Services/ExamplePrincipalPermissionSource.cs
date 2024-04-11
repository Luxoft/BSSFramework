using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.DiTests;

public class ExamplePrincipalPermissionSource : IPrincipalPermissionSource<Guid>
{
    private readonly List<Dictionary<Type, List<Guid>>> permissions;

    public ExamplePrincipalPermissionSource(List<Dictionary<Type, List<Guid>>> permissions)
    {
        this.permissions = permissions;
    }

    public List<Dictionary<Type, List<Guid>>> GetPermissions()
    {
        return this.permissions;
    }

    public IQueryable<IPermission<Guid>> GetPermissionQuery(SecurityRule securityRule)
    {
        throw new NotImplementedException();
    }
}
