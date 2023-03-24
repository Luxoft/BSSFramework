namespace Framework.SecuritySystem;

public interface IPermissionFilterItem<out TIdent>
{
    IPermissionFilterEntity<TIdent> Entity { get; }

    TIdent ContextEntityId { get; }

    IEntityType<TIdent> EntityType { get; }
}
