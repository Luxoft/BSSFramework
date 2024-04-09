using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public interface ISecurityRoleParser
{
    IReadOnlyList<SecurityRule> Operations { get; }

    SecurityRule Parse(string name);
}
