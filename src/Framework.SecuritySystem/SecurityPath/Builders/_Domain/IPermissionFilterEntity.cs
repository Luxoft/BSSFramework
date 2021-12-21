namespace Framework.SecuritySystem
{
    public interface IPermissionFilterEntity<out TIdent>
    {
        IEntityType EntityType { get; }

        TIdent EntityId { get; }
    }
}