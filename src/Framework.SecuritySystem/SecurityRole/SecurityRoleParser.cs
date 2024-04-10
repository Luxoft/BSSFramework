namespace Framework.SecuritySystem;

public class SecurityRoleParser : ISecurityRoleParser
{
    private readonly IReadOnlyDictionary<Guid, SecurityRole> securityRoleByIdDict;

    private readonly IReadOnlyDictionary<string, SecurityRole> securityRoleByNameDict;

    public SecurityRoleParser(ISecurityRoleSource securityRoleSource)
    {
        this.Roles = securityRoleSource.SecurityRoles;

        this.securityRoleByIdDict = this.Roles.ToDictionary(v => v.Id);

        this.securityRoleByNameDict = this.Roles.ToDictionary(v => v.Name);
    }

    public IReadOnlyList<SecurityRole> Roles { get; }

    public SecurityRole Parse(string name)
    {
        return this.securityRoleByNameDict.GetValueOrDefault(name)
               ?? throw new Exception($"SecurityRole with name '{name}' not found");
    }

    public SecurityRole GetSecurityRole(Guid id)
    {
        return this.securityRoleByIdDict.GetValueOrDefault(id) ?? throw new Exception($"SecurityRole with id '{id}' not found");
    }
}
