namespace Framework.SecuritySystem;

public interface IPermissionFilterEntity<out TIdent>
{
    IEntityType<TIdent> EntityType { get; }

    TIdent EntityId { get; }
}
