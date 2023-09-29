using Framework.Security;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class SecurityOperationParser<TIdent> : ISecurityOperationParser<TIdent>
{
    private readonly IReadOnlyDictionary<TIdent, SecurityOperation> securityOperationByIdDict;

    private readonly IReadOnlyDictionary<string, SecurityOperation> securityOperationByNameDict;

    public SecurityOperationParser(IEnumerable<SecurityOperationTypeInfo> securityOperationTypeInfos)
    {
        this.Operations = securityOperationTypeInfos
                            .SelectMany(v => SecurityOperationHelper.GetSecurityOperations(v.SecurityOperationType))
                            .Where(op => op is not DisabledSecurityOperation)
                            .Distinct()
                            .ToList();

        this.securityOperationByNameDict = this.Operations.ToDictionary(v => v.Name);

        this.securityOperationByIdDict = this.Operations.ToDictionary(v => ((SecurityOperation<TIdent>)v).Id);
    }

    public IReadOnlyList<SecurityOperation> Operations { get; }

    public SecurityOperation Parse(string name)
    {
        return this.securityOperationByNameDict.GetValueOrDefault(name)
               ?? throw new Exception($"SecurityOperation with name '{name}' not found");
    }

    public SecurityOperation GetSecurityOperation(TIdent id)
    {
        return this.securityOperationByIdDict.GetValueOrDefault(id) ?? throw new Exception($"SecurityOperation with id '{id}' not found");
    }
}
