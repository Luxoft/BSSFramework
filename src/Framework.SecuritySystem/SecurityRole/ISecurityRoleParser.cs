namespace Framework.SecuritySystem;

public interface ISecurityRoleParser
{
    IReadOnlyList<SecurityRole> Roles { get; }

    SecurityRole Parse(string name);

    SecurityRole GetSecurityRole(Guid id);
}
