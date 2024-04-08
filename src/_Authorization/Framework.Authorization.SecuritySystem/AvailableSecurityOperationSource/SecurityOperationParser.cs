using Framework.Security;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class SecurityOperationParser : ISecurityOperationParser
{
    private readonly IReadOnlyDictionary<string, SecurityRule> securityOperationByNameDict;

    public SecurityOperationParser(IEnumerable<SecurityOperationTypeInfo> securityOperationTypeInfos)
    {
        this.Operations = securityOperationTypeInfos
                            .SelectMany(v => SecurityOperationHelper.GetSecurityOperations(v.SecurityOperationType))
                            .Where(op => op != SecurityRule.Disabled)
                            .Cast<SecurityRule>()
                            .Distinct()
                            .ToList();

        this.securityOperationByNameDict = this.Operations.ToDictionary(v => v.Name);
    }

    public IReadOnlyList<SecurityRule> Operations { get; }

    public SecurityRule Parse(string name)
    {
        return this.securityOperationByNameDict.GetValueOrDefault(name)
               ?? throw new Exception($"SecurityRule with name '{name}' not found");
    }

    SecurityRule ISecurityOperationParser.Parse(string name) => this.Parse(name);

    IReadOnlyList<SecurityRule> ISecurityOperationParser.Operations => this.Operations;
}
