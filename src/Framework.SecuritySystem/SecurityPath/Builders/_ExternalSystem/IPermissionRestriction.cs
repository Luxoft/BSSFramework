namespace Framework.SecuritySystem.ExternalSystem;

public interface IPermissionRestriction<out TIdent>
{
    TIdent SecurityContextTypeId { get; }

    TIdent SecurityContextId { get; }
}
