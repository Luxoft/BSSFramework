using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public interface ISecurityOperationParser
{
    IReadOnlyList<SecurityOperation> Operations { get; }

    SecurityOperation Parse(string name);
}

public interface ISecurityOperationParser<TIdent> : ISecurityOperationParser
{
    new IReadOnlyList<SecurityOperation<TIdent>> Operations { get; }

    new SecurityOperation<TIdent> Parse(string name);

    SecurityOperation<TIdent> GetSecurityOperation(TIdent id);
}
