using Anch.GenericQueryable.Fetching;
using Anch.OData.Domain;

using Framework.Application.Domain;
using Framework.BLL.Domain.Persistent;
using Framework.Database;

namespace Framework.BLL;

public interface IDefaultDomainBLLQueryBase<in TPersistentDomainObjectBase, TDomainObject, TIdent> :
        IBLLQueryBase<TDomainObject>, IRevisionBLL<TDomainObject, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
{
    TDomainObject? GetById(TIdent id, IdCheckMode idCheckMode, FetchRule<TDomainObject>? fetchRule = null, LockRole lockRole = LockRole.None);

    TDomainObject? GetById(TIdent id, IdCheckMode idCheckMode, Func<PropertyFetchRule<TDomainObject>, FetchRule<TDomainObject>> buildFetchRule);

    TDomainObject? GetById(TIdent id, bool throwOnNotFound = false, FetchRule<TDomainObject>? fetchRule = null, LockRole lockRole = LockRole.None);

    TDomainObject? GetById(TIdent id, bool throwOnNotFound, Func<PropertyFetchRule<TDomainObject>, FetchRule<TDomainObject>> buildFetchRule);

    List<TDomainObject> GetListByIdents(IEnumerable<TIdent> baseIdents, FetchRule<TDomainObject>? fetchRule = null);

    List<TDomainObject> GetListByIdents(IEnumerable<TIdent> baseIdents, Func<PropertyFetchRule<TDomainObject>, FetchRule<TDomainObject>> buildFetchRule);

    List<TDomainObject> GetListByIdentsUnsafe(IEnumerable<TIdent> baseIdents, FetchRule<TDomainObject>? fetchRule = null);

    List<TDomainObject> GetListByIdentsUnsafe(IEnumerable<TIdent> baseIdents, Func<PropertyFetchRule<TDomainObject>, FetchRule<TDomainObject>> buildFetchRule);


    List<TDomainObject> GetListByIdents<TIdentity>(IEnumerable<TIdentity> idents, FetchRule<TDomainObject>? fetchRule = null)
            where TIdentity : IIdentityObject<TIdent>;

    TDomainObject GetNested(TDomainObject domainObject);

    /// <summary>
    /// Getting an object tree
    /// </summary>
    /// <param name="fetchRule">Properties to fetch</param>
    /// <returns></returns>
    List<HierarchicalNode<TDomainObject, TIdent>> GetTree(FetchRule<TDomainObject>? fetchRule = null);

    /// <summary>
    /// Getting an object tree by an OData query
    /// </summary>
    /// <param name="selectOperation">OData query</param>
    /// <param name="fetchRule">Properties to fetch</param>
    /// <returns></returns>
    SelectOperationResult<HierarchicalNode<TDomainObject, TIdent>> GetTreeByOData(SelectOperation<TDomainObject> selectOperation, FetchRule<TDomainObject>? fetchRule = null);
}


public interface IDefaultDomainBLLQueryBase<out TBLLContext, in TPersistentDomainObjectBase, TDomainObject, TIdent> :

        IDefaultDomainBLLQueryBase<TPersistentDomainObjectBase, TDomainObject, TIdent>,
        IBLLBase<TBLLContext, TDomainObject>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase;

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
        where TDomainObject : class, TPersistentDomainObjectBase;

public interface IDefaultDomainBLLBase<out TBLLContext, in TPersistentDomainObjectBase, TDomainObject, TIdent> :

        IDefaultDomainBLLBase<TPersistentDomainObjectBase, TDomainObject, TIdent>,
        IDefaultDomainBLLQueryBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TIdent>

        where TBLLContext : IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase;
