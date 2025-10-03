using SecuritySystem;

namespace Automation.ServiceEnvironment;

public record AdministratorsRoleList(IReadOnlyList<SecurityRole> Roles)
{
    public static AdministratorsRoleList Default { get; } = new([SecurityRole.Administrator, SecurityRole.SystemIntegration]);
}
