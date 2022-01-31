namespace Framework.Persistent;

/// <summary>
/// Иерархический объект с идентефикатором
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TIdent"></typeparam>
public interface IHierarchicalPersistentDomainObjectBase<out T, out TIdent> : IIdentityObject<TIdent>, IHierarchicalSource<T>
        where T : IHierarchicalPersistentDomainObjectBase<T, TIdent>
{
}
