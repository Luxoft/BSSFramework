using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

using Framework.Core;
using Framework.OData;
using Framework.Persistent;

#nullable enable

namespace Framework.DomainDriven.BLL;

public partial interface IDefaultDomainBLLQueryBase<in TPersistentDomainObjectBase, TDomainObject, TIdent> :
        IBLLQueryBase<TDomainObject>, IRevisionBLL<TDomainObject, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
{
    TDomainObject? GetById(TIdent id, IdCheckMode idCheckMode, IFetchContainer<TDomainObject>? fetchContainer = null, LockRole lockRole = LockRole.None);

    TDomainObject? GetById(TIdent id, IdCheckMode idCheckMode, Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs);

    TDomainObject? GetById(TIdent id, IdCheckMode idCheckMode, IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs);


    TDomainObject? GetById(TIdent id, [DoesNotReturnIf(true)] bool throwOnNotFound = false, IFetchContainer<TDomainObject>? fetchContainer = null, LockRole lockRole = LockRole.None);

    TDomainObject? GetById(TIdent id,  [DoesNotReturnIf(true)] bool throwOnNotFound, Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs);

    TDomainObject? GetById(TIdent id, [DoesNotReturnIf(true)] bool throwOnNotFound, IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs);



    List<TDomainObject> GetListByIdents(IEnumerable<TIdent> baseIdents, IFetchContainer<TDomainObject>? fetchContainer = null);

    List<TDomainObject> GetListByIdents(IEnumerable<TIdent> baseIdents, Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs);

    List<TDomainObject> GetListByIdents(IEnumerable<TIdent> baseIdents, IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs);


    List<TDomainObject> GetListByIdentsUnsafe(IEnumerable<TIdent> baseIdents, IFetchContainer<TDomainObject>? fetchContainer = null);

    List<TDomainObject> GetListByIdentsUnsafe(IEnumerable<TIdent> baseIdents, Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs);

    List<TDomainObject> GetListByIdentsUnsafe(IEnumerable<TIdent> baseIdents, IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs);


    List<TDomainObject> GetListByIdents<TIdentity>(IEnumerable<TIdentity> idents, IFetchContainer<TDomainObject>? fetchContainer = null)
            where TIdentity : IIdentityObject<TIdent>;

    TDomainObject GetNested(TDomainObject domainObject);

    /// <summary>
    /// Получение дерева объектов
    /// </summary>
    /// <param name="fetchs">Подгружаемые свойства</param>
    /// <returns></returns>
    List<HierarchicalNode<TDomainObject, TIdent>> GetTree(IFetchContainer<TDomainObject>? fetchs = null);

    /// <summary>
    /// Получение дерева объектов по OData-запросу
    /// </summary>
    /// <param name="selectOperation">OData-запрос</param>
    /// <param name="fetchs">Подгружаемые свойства</param>
    /// <returns></returns>
    SelectOperationResult<HierarchicalNode<TDomainObject, TIdent>> GetTreeByOData(SelectOperation<TDomainObject> selectOperation, IFetchContainer<TDomainObject>? fetchs = null);
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

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
{
}

public interface IDefaultDomainBLLBase<out TBLLContext, in TPersistentDomainObjectBase, TDomainObjectBase, TDomainObject, TIdent> :

        IDefaultDomainBLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TIdent>

        where TBLLContext : IDefaultBLLContext<TPersistentDomainObjectBase, TDomainObjectBase, TIdent>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>, TDomainObjectBase
        where TDomainObjectBase : class
        where TDomainObject : class, TPersistentDomainObjectBase
{
}
