namespace Framework.SecuritySystem.DependencyInjection;

public interface IInitializedSecurityRoleSource
{
    IEnumerable<FullSecurityRole> GetSecurityRoles();
}
