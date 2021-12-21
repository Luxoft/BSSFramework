namespace Framework.SecuritySystem
{
    public interface IPermissionFilterItem<out TIdent>
    {
        IPermissionFilterEntity<TIdent> Entity { get; }
    }
}