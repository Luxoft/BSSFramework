using Framework.Security;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class SecurityOperationParser : ISecurityOperationParser
{
    private readonly IReadOnlyDictionary<string, SecurityOperation> securityOperationByNameDict;

    public SecurityOperationParser(IEnumerable<SecurityOperationTypeInfo> securityOperationTypeInfos)
    {
        this.Operations = securityOperationTypeInfos
                            .SelectMany(v => SecurityOperationHelper.GetSecurityOperations(v.SecurityOperationType))
                            .Where(op => op != SecurityOperation.Disabled)
                            .Cast<SecurityOperation>()
                            .Distinct()
                            .ToList();

        this.securityOperationByNameDict = this.Operations.ToDictionary(v => v.Name);
    }

    public IReadOnlyList<SecurityOperation> Operations { get; }

    public SecurityOperation Parse(string name)
    {
        return this.securityOperationByNameDict.GetValueOrDefault(name)
               ?? throw new Exception($"SecurityOperation with name '{name}' not found");
    }

    SecurityOperation ISecurityOperationParser.Parse(string name) => this.Parse(name);

    IReadOnlyList<SecurityOperation> ISecurityOperationParser.Operations => this.Operations;
}
