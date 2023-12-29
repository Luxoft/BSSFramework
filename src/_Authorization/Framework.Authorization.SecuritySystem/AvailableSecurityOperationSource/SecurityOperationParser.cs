using Framework.Security;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class SecurityOperationParser<TIdent> : ISecurityOperationParser<TIdent>
    where TIdent : notnull
{
    private readonly IReadOnlyDictionary<TIdent, SecurityOperation<TIdent>> securityOperationByIdDict;

    private readonly IReadOnlyDictionary<string, SecurityOperation<TIdent>> securityOperationByNameDict;

    public SecurityOperationParser(IEnumerable<SecurityOperationTypeInfo> securityOperationTypeInfos)
    {
        this.Operations = securityOperationTypeInfos
                            .SelectMany(v => SecurityOperationHelper.GetSecurityOperations(v.SecurityOperationType))
                            .Where(op => op is not DisabledSecurityOperation)
                            .Cast<SecurityOperation<TIdent>>()
                            .Distinct()
                            .ToList();

        this.securityOperationByNameDict = this.Operations.ToDictionary(v => v.Name);

        this.securityOperationByIdDict = this.Operations.ToDictionary(v => v.Id);
    }

    public IReadOnlyList<SecurityOperation<TIdent>> Operations { get; }

    public SecurityOperation<TIdent> Parse(string name)
    {
        return this.securityOperationByNameDict.GetValueOrDefault(name)
               ?? throw new Exception($"SecurityOperation with name '{name}' not found");
    }

    public SecurityOperation<TIdent> GetSecurityOperation(TIdent id)
    {
        return this.securityOperationByIdDict.GetValueOrDefault(id) ?? throw new Exception($"SecurityOperation with id '{id}' not found");
    }

    SecurityOperation ISecurityOperationParser.Parse(string name) => this.Parse(name);

    IReadOnlyList<SecurityOperation> ISecurityOperationParser.Operations => this.Operations;
}
