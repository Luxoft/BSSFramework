using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public interface ISecurityOperationParser
{
    IReadOnlyList<SecurityOperation> Operations { get; }

    SecurityOperation Parse(string name);
}

public interface ISecurityOperationParser<in TIdent> : ISecurityOperationParser
{
    SecurityOperation GetSecurityOperation(TIdent id);
}
