using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public interface ISecurityOperationParser
{
    IReadOnlyList<SecurityRule> Operations { get; }

    SecurityRule Parse(string name);
}
