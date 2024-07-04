#nullable enable
namespace Framework.SecuritySystem;

public class FullSecurityRole(string name, SecurityRoleInfo information) : SecurityRole(name)
{
    public SecurityRoleInfo Information { get; } = information;

    public Guid Id => this.Information.Id;

    public bool IsVirtual => this.Id == default;
}
