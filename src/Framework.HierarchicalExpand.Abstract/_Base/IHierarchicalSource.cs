namespace Framework.Persistent
{
    public interface IHierarchicalSource<out T> : IParentSource<T>, IChildrenSource<T>
    {
    }
}
