using System.Diagnostics.CodeAnalysis;

using Framework.DomainDriven.Lock;
using Framework.OData;
using Framework.Persistent;

using GenericQueryable.Fetching;

namespace Framework.DomainDriven.BLL;

public interface IDefaultDomainBLLQueryBase<in TPersistentDomainObjectBase, TDomainObject, TIdent> :
        IBLLQueryBase<TDomainObject>, IRevisionBLL<TDomainObject, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
{
    TDomainObject? GetById(TIdent id, IdCheckMode idCheckMode, FetchRule<TDomainObject>? fetchRule = null, LockRole lockRole = LockRole.None);

    TDomainObject? GetById(TIdent id, IdCheckMode idCheckMode, Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule);

    [return: NotNullIfNotNull(nameof(throwOnNotFound))]
    TDomainObject? GetById(TIdent id, bool throwOnNotFound = false, FetchRule<TDomainObject>? fetchRule = null, LockRole lockRole = LockRole.None);

    [return: NotNullIfNotNull(nameof(throwOnNotFound))]
    TDomainObject? GetById(TIdent id, bool throwOnNotFound, Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule);

    List<TDomainObject> GetListByIdents(IEnumerable<TIdent> baseIdents, FetchRule<TDomainObject>? fetchRule = null);

    List<TDomainObject> GetListByIdents(IEnumerable<TIdent> baseIdents, Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule);

    List<TDomainObject> GetListByIdentsUnsafe(IEnumerable<TIdent> baseIdents, FetchRule<TDomainObject>? fetchRule = null);

    List<TDomainObject> GetListByIdentsUnsafe(IEnumerable<TIdent> baseIdents, Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule);


    List<TDomainObject> GetListByIdents<TIdentity>(IEnumerable<TIdentity> idents, FetchRule<TDomainObject>? fetchRule = null)
            where TIdentity : IIdentityObject<TIdent>;

    TDomainObject GetNested(TDomainObject domainObject);

    /// <summary>
    /// Получение дерева объектов
    /// </summary>
    /// <param name="fetchRule">Подгружаемые свойства</param>
    /// <returns></returns>
    List<HierarchicalNode<TDomainObject, TIdent>> GetTree(FetchRule<TDomainObject>? fetchRule = null);

    /// <summary>
    /// Получение дерева объектов по OData-запросу
    /// </summary>
    /// <param name="selectOperation">OData-запрос</param>
    /// <param name="fetchRule">Подгружаемые свойства</param>
    /// <returns></returns>
    SelectOperationResult<HierarchicalNode<TDomainObject, TIdent>> GetTreeByOData(SelectOperation<TDomainObject> selectOperation, FetchRule<TDomainObject>? fetchRule = null);
}


public interface IDefaultDomainBLLQueryBase<out TBLLContext, in TPersistentDomainObjectBase, TDomainObject, TIdent> :

        IDefaultDomainBLLQueryBase<TPersistentDomainObjectBase, TDomainObject, TIdent>,
        IBLLBase<TBLLContext, TDomainObject>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
{
}

public interface IDefaultDomainBLLBase<in TDomainObject, in TIdent> : IOperationBLLBase<TDomainObject>
        where TDomainObject : IIdentityObject<TIdent>
{
    void Insert(TDomainObject domainObject, TIdent id);

    void Lock(TDomainObject domainObject, LockRole lockRole);
}

public interface IDefaultDomainBLLBase<in TPersistentDomainObjectBase, TDomainObject, TIdent> :
        IDefaultDomainBLLBase<TDomainObject, TIdent>,
        IDefaultDomainBLLQueryBase<TPersistentDomainObjectBase, TDomainObject, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
{
}

public interface IDefaultDomainBLLBase<out TBLLContext, in TPersistentDomainObjectBase, TDomainObject, TIdent> :

        IDefaultDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent>,
        IDefaultDomainBLLQueryBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TIdent>

        where TBLLContext : IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
{
}
