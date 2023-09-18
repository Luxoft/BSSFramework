namespace Framework.SecuritySystem;

public interface ISecurityOperation<out TIdent> : ISecurityOperation
{
    TIdent Id { get; }
}
