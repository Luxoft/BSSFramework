namespace Framework.Persistent
{
    public interface IIdentityObject<out TIdent>
    {
        TIdent Id { get; }
    }
}
