using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.DiTests;

public class ExamplePermissionSource(ExamplePermissionSystemData data) : IPermissionSource
{
    public bool HasAccess() => throw new NotImplementedException();

    public List<Dictionary<Type, List<Guid>>> GetPermissions(IEnumerable<Type> securityTypes) => data.Permissions;
}
