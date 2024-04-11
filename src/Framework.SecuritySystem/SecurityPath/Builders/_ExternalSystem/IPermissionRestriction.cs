namespace Framework.SecuritySystem.ExternalSystem;

public interface IPermissionRestriction<out TIdent>
{
    ISecurityContextType<TIdent> SecurityContextType { get; }

    TIdent SecurityContextId { get; }
}
