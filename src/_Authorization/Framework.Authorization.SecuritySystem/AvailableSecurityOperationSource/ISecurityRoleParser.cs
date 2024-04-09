using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public interface ISecurityRoleParser
{
    IReadOnlyList<SecurityRole> Roles { get; }

    SecurityRole Parse(string name);

    SecurityRole GetSecurityRole(Guid id);
}
