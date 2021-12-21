namespace Framework.Persistent
{
    public interface IIdentityObjectContainer<out TIdentityObject>
    {
        TIdentityObject Identity { get; }
    }
}