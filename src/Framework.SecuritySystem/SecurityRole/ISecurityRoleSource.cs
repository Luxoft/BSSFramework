namespace Framework.SecuritySystem;

public interface ISecurityRoleSource
{
    IReadOnlyList<FullSecurityRole> SecurityRoles { get; }

    FullSecurityRole GetFullRole(SecurityRole securityRole);

    FullSecurityRole GetSecurityRole(string name);

    FullSecurityRole GetSecurityRole(Guid id);

    IEnumerable<FullSecurityRole> GetRealRoles() => this.SecurityRoles.Where(sr => !sr.IsVirtual);
}
