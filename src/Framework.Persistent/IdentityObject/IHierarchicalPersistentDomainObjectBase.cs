namespace Framework.Persistent;

public interface IHierarchicalPersistentDomainObjectBase<out T, out TIdent> : IParentSource<T>, IIdentityObject<TIdent>;
