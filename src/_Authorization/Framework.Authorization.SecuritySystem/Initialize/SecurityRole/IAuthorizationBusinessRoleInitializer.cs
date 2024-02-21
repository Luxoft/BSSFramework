namespace Framework.Authorization.SecuritySystem.Initialize;

public interface IAuthorizationBusinessRoleInitializer : ISecurityInitializer
{
    Task Init(IEnumerable<SecurityRole> securityRoles, CancellationToken cancellationToken = default);
}
