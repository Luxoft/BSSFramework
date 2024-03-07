using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

using Framework.Core;
using Framework.DomainDriven.Lock;
using Framework.Exceptions;
using Framework.HierarchicalExpand;
using Framework.OData;
using Framework.Persistent;

using nuSpec.Abstraction;

#nullable enable

namespace Framework.DomainDriven.BLL;

public abstract class DefaultDomainBLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TIdent> :
        BLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TIdent>,
        IDefaultDomainBLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
        where TBLLContext : class, IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>, IHierarchicalObjectExpanderFactoryContainer<TIdent>, IBLLBaseContext<TPersistentDomainObjectBase, TIdent>
{
    private const int MaxItemsInSql = 2000;

    protected DefaultDomainBLLBase(TBLLContext context, ISpecificationEvaluator? specificationEvaluator = null)
            : base(context, specificationEvaluator)
    {
    }


    public TDomainObject? GetById(TIdent id, IdCheckMode idCheckMode, IFetchContainer<TDomainObject>? fetchContainer = null, LockRole lockRole = LockRole.None) =>
            idCheckMode switch
            {
                    IdCheckMode.DontCheck => this.GetById(id, false, fetchContainer, lockRole),
                    IdCheckMode.CheckAll => this.GetById(id, true, fetchContainer, lockRole),
                    IdCheckMode.SkipEmpty => id.IsDefault() ? null : this.GetById(id, true, fetchContainer, lockRole),
                    _ => throw new ArgumentOutOfRangeException(nameof(idCheckMode))
            };

    public TDomainObject? GetById(
            TIdent id,
            IdCheckMode idCheckMode,
            Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch,
            params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs)
    {
        if (firstFetch == null)
        {
            throw new ArgumentNullException(nameof(firstFetch));
        }

        if (otherFetchs == null)
        {
            throw new ArgumentNullException(nameof(otherFetchs));
        }

        return this.GetById(id, idCheckMode, new[] { firstFetch }.Concat(otherFetchs));
    }

    public TDomainObject? GetById(TIdent id, IdCheckMode idCheckMode, IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs)
    {
        if (fetchs == null)
        {
            throw new ArgumentNullException(nameof(fetchs));
        }

        return this.GetById(id, idCheckMode, fetchs.ToFetchContainer());
    }

    public TDomainObject GetNested(TDomainObject domainObject)
    {
        var method = new Func<TDomainObject, TDomainObject>(this.GetNested<TDomainObject>).Method.GetGenericMethodDefinition();

        var request = from t in typeof(TDomainObject).Assembly.GetTypes()
                      where t != typeof(TDomainObject) && !t.IsAbstract && !t.IsGenericTypeDefinition && typeof(TDomainObject).IsAssignableFrom(t)
                      let nestedDomainObject =
                              t.IsInstanceOfType(domainObject) ? domainObject : method.MakeGenericMethod(t).Invoke<TDomainObject>(this, new[] { domainObject })
                      where nestedDomainObject != null
                      select nestedDomainObject;

        return request.Single(() => new Exception($"Can't find nested type for {typeof(TDomainObject).Name}"));
    }

    /// <inheritdoc />
    public virtual List<HierarchicalNode<TDomainObject, TIdent>> GetTree(IFetchContainer<TDomainObject>? fetchs = null) =>
            this.GetTree(this.GetFullList(fetchs), _ => this.GetSecureQueryable().Select(domainObject => domainObject.Id), this.ExpandQueryableWithParents);

    /// <inheritdoc />
    public virtual SelectOperationResult<HierarchicalNode<TDomainObject, TIdent>> GetTreeByOData(
            SelectOperation<TDomainObject> selectOperation,
            IFetchContainer<TDomainObject>? fetchs = null)
    {
        if (selectOperation == null)
        {
            throw new ArgumentNullException(nameof(selectOperation));
        }

        var odataResult = this.GetObjectsByOData(selectOperation, fetchs);

        var tree = this.GetTree(odataResult.Items, x => x.ToList(domainObject => domainObject.Id), this.ExpandEnumerableWithParents);

        return new SelectOperationResult<HierarchicalNode<TDomainObject, TIdent>>(tree, tree.Count);
    }

    private List<HierarchicalNode<TDomainObject, TIdent>> GetTree<TIdentCollection>(
            List<TDomainObject> startProjections,
            Func<IEnumerable<TDomainObject>, TIdentCollection> identsSelector,
            Func<TIdentCollection, HierarchicalExpandType, Dictionary<TIdent, TIdent>> parentsExpander,
            IFetchContainer<TDomainObject>? fetchs = null)
            where TIdentCollection : IEnumerable<TIdent>
    {
        if (startProjections == null)
        {
            throw new ArgumentNullException(nameof(startProjections));
        }

        var projectionsIdents = identsSelector(startProjections);

        var parentExpandMode = this.Context.AllowedExpandTreeParents<TDomainObject>() ? HierarchicalExpandType.Parents : HierarchicalExpandType.None;

        var allIdentDict = parentsExpander(projectionsIdents, parentExpandMode);

        var expandedParents = allIdentDict.Keys.Except(projectionsIdents).ToList();

        var parents = this.Context.Logics.Default.Create<TDomainObject>().GetListByIdents(expandedParents, fetchs);

        return startProjections.Select(item => new { Item = item, OnlyView = false })
                               .Concat(parents.Select(item => new { Item = item, OnlyView = true }))
                               .ToList(
                                       pair => new HierarchicalNode<TDomainObject, TIdent>
                                               {
                                                       Item = pair.Item,
                                                       OnlyView = pair.OnlyView,
                                                       ParentId = allIdentDict[pair.Item.Id]
                                               });
    }

    private Dictionary<TIdent, TIdent> ExpandEnumerableWithParents(IEnumerable<TIdent> projectionsIdents, HierarchicalExpandType parentExpandMode)
    {
        var hierarchicalObjectExpander = this.Context.HierarchicalObjectExpanderFactory.Create(typeof(TDomainObject));
        return projectionsIdents.Split(MaxItemsInSql)
                                .SelectMany(z => hierarchicalObjectExpander.ExpandWithParents(z, parentExpandMode))
                                .Distinct()
                                .ToDictionary(z => z.Key, z => z.Value);
    }

    private Dictionary<TIdent, TIdent> ExpandQueryableWithParents(IQueryable<TIdent> projectionsIdents, HierarchicalExpandType parentExpandMode)
    {
        return this.Context.HierarchicalObjectExpanderFactory.Create(typeof(TDomainObject)).ExpandWithParents(projectionsIdents, parentExpandMode);
    }

    public List<TDomainObject> GetListByIdents<TIdentity>(IEnumerable<TIdentity> idents, IFetchContainer<TDomainObject>? fetchContainer = null)
            where TIdentity : IIdentityObject<TIdent>
    {
        return this.GetListByIdents(idents.Select(ident => ident.Id), fetchContainer);
    }

    public List<TDomainObject> GetListByIdents(IEnumerable<TIdent> baseIdents, IFetchContainer<TDomainObject>? fetchContainer = null)
    {
        if (baseIdents == null)
        {
            throw new ArgumentNullException(nameof(baseIdents));
        }

        var idents = baseIdents.ToList();

        var uniqueIdents = idents.Distinct().ToList();

        var uniqueResult = uniqueIdents.Split(MaxItemsInSql).SelectMany(path => this.GetListBy(v => path.Contains(v.Id), fetchContainer)).ToList();

        if (uniqueResult.Count == uniqueIdents.Count)
        {
            var resultRequest = from ident in idents
                                join domainObject in uniqueResult on ident equals domainObject.Id
                                select domainObject;

            return resultRequest.ToList();
        }

        throw uniqueIdents.Except(uniqueResult.Select(v => v.Id)).Select(this.GetMissingObjectException).Aggregate();
    }

    public List<TDomainObject> GetListByIdents(
            IEnumerable<TIdent> baseIdents,
            Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch,
            params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs)
    {
        if (baseIdents == null)
        {
            throw new ArgumentNullException(nameof(baseIdents));
        }

        if (firstFetch == null)
        {
            throw new ArgumentNullException(nameof(firstFetch));
        }

        if (otherFetchs == null)
        {
            throw new ArgumentNullException(nameof(otherFetchs));
        }

        return this.GetListByIdents(baseIdents, new[] { firstFetch }.Concat(otherFetchs));
    }

    public List<TDomainObject> GetListByIdents(IEnumerable<TIdent> baseIdents, IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs)
    {
        if (baseIdents == null)
        {
            throw new ArgumentNullException(nameof(baseIdents));
        }

        if (fetchs == null)
        {
            throw new ArgumentNullException(nameof(fetchs));
        }

        return this.GetListByIdents(baseIdents, fetchs.ToFetchContainer());
    }

    public override SelectOperationResult<TDomainObject> GetObjectsByOData(
            SelectOperation selectOperation,
            IFetchContainer<TDomainObject>? fetchContainer = null)
    {
        var typedSelectOperation = this.Context.StandartExpressionBuilder.ToTyped<TDomainObject>(selectOperation);

        return this.GetObjectsByOData(typedSelectOperation, fetchContainer);
    }

    /// <summary>
    /// For Projections (reports)
    /// </summary>
    /// <remarks>
    /// Метод пока что не используется и это походу проблема #IADFRAME-943
    /// </remarks>
    public override SelectOperationResult<TProjection> GetObjectsByOData<TProjection>(SelectOperation<TProjection> selectOperation, Expression<Func<TDomainObject, TProjection>> projectionSelector)
    {
        if (selectOperation == null)
        {
            throw new ArgumentNullException(nameof(selectOperation));
        }

        if (selectOperation.HasPaging)
        {
            var countQuery = this.GetSecureQueryable().Select(projectionSelector).Pipe(z => selectOperation.ToCountOperation().Process(z));

            var selection = this.GetSecureQueryable()
                                .Select(projectionSelector)
                                .Pipe(z => selectOperation.WithoutPaging().Process(z))
                                .Select(domainObject => ((IIdentityObject<TIdent>)domainObject).Id);

            if (!selectOperation.Orders.Any())
            {
                selection = selection.OrderBy(x => x);
            }

            var idents = selection
                         .Skip(selectOperation.SkipCount)
                         .Take(selectOperation.TakeCount);

            var result = this.GetListByIdentsNoSecurable(idents, projectionSelector);

            var count = countQuery.Count();

            return new SelectOperationResult<TProjection>(result, count);
        }
        else
        {
            var result = this.GetSecureQueryable().Select(projectionSelector).Pipe(selectOperation.Process);

            return new SelectOperationResult<TProjection>(result);
        }
    }

    public override SelectOperationResult<TDomainObject> GetObjectsByOData(
            SelectOperation<TDomainObject> selectOperation,
            IFetchContainer<TDomainObject>? fetchContainer = null)
    {
        if (selectOperation == null)
        {
            throw new ArgumentNullException(nameof(selectOperation));
        }

        if (!this.Context.AllowVirtualPropertyInOdata(typeof(TDomainObject)) && selectOperation.IsVirtual)
        {
            throw new ArgumentException(
                                        $"GetObjectsByOData: It's not allowed to use virtual property in OData select query: {selectOperation.Filter.Body}. You should either allow using virtual properties by overriding AllowVirtualPropertyInOdata property in BLL or don’t use them in OData query.");
        }

        if (selectOperation.HasPaging)
        {
            var countQuery = this.GetSecureQueryable(selectOperation.ToCountOperation());

            var selection = this.GetSecureQueryable(selectOperation.WithoutPaging())
                                .Select(domainObject => domainObject.Id);

            if (!selectOperation.Orders.Any())
            {
                selection = selection.OrderBy(x => x);
            }

            var idents =
                    selection
                            .Skip(selectOperation.SkipCount)
                            .Take(selectOperation.TakeCount);

            var result = this.GetListByIdentsNoSecurable(idents, fetchContainer);

            var count = countQuery.Count();

            return new SelectOperationResult<TDomainObject>(result, count);
        }
        else
        {
            var result = this.GetSecureQueryable(selectOperation, fetchContainer);

            return new SelectOperationResult<TDomainObject>(result);
        }
    }

    public List<TDomainObject> GetListByIdentsUnsafe(IEnumerable<TIdent> baseIdents, IFetchContainer<TDomainObject>? fetchContainer = null)
    {
        if (baseIdents == null)
        {
            throw new ArgumentNullException(nameof(baseIdents));
        }

        return baseIdents.Distinct().Split(MaxItemsInSql).SelectMany(path => this.GetListBy(v => path.Contains(v.Id), fetchContainer)).ToList();
    }

    public List<TDomainObject> GetListByIdentsUnsafe(
            IEnumerable<TIdent> baseIdents,
            Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch,
            params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs)
    {
        if (baseIdents == null)
        {
            throw new ArgumentNullException(nameof(baseIdents));
        }

        if (firstFetch == null)
        {
            throw new ArgumentNullException(nameof(firstFetch));
        }

        if (otherFetchs == null)
        {
            throw new ArgumentNullException(nameof(otherFetchs));
        }

        return this.GetListByIdentsUnsafe(baseIdents, new[] { firstFetch }.Concat(otherFetchs));
    }

    public List<TDomainObject> GetListByIdentsUnsafe(
            IEnumerable<TIdent> baseIdents,
            IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs)
    {
        if (baseIdents == null)
        {
            throw new ArgumentNullException(nameof(baseIdents));
        }

        if (fetchs == null)
        {
            throw new ArgumentNullException(nameof(fetchs));
        }

        return this.GetListByIdentsUnsafe(baseIdents, fetchs.ToFetchContainer());
    }

    public TDomainObject? GetById(TIdent id, [DoesNotReturnIf(true)] bool throwOnNotFound = false, IFetchContainer<TDomainObject>? fetchContainer = null, LockRole lockRole = LockRole.None)
    {
        var result = this.GetListBy(domainObject => domainObject.Id.Equals(id), fetchContainer, lockRole).FirstOrDefault();

        if (result == null && throwOnNotFound)
        {
            throw this.GetMissingObjectException(id);
        }

        return result;
    }

    public TDomainObject? GetById(
            TIdent id,
            [DoesNotReturnIf(true)] bool throwOnNotFound,
            Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch,
            params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs)
    {
        if (firstFetch == null)
        {
            throw new ArgumentNullException(nameof(firstFetch));
        }

        if (otherFetchs == null)
        {
            throw new ArgumentNullException(nameof(otherFetchs));
        }

        return this.GetById(id, throwOnNotFound, new[] { firstFetch }.Concat(otherFetchs));
    }

    public TDomainObject? GetById(TIdent id,[DoesNotReturnIf(true)] bool throwOnNotFound, IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs)
    {
        if (fetchs == null)
        {
            throw new ArgumentNullException(nameof(fetchs));
        }

        return this.GetById(id, throwOnNotFound, fetchs.ToFetchContainer());
    }

    protected List<TDomainObject> GetListByIdentsQueryable(IQueryable<TIdent> baseIdents, IFetchContainer<TDomainObject>? fetchContainer = null)
    {
        if (baseIdents == null)
        {
            throw new ArgumentNullException(nameof(baseIdents));
        }

        var result = this.GetListBy(v => baseIdents.Contains(v.Id), fetchContainer).ToList();

        return result;
    }

    protected List<TProjection> GetListByIdentsNoSecurable<TProjection>(
            IEnumerable<TIdent> baseIdents,
            Expression<Func<TDomainObject, TProjection>> projectionSelector,
            IFetchContainer<TDomainObject>? fetchContainer = null)
    {
        if (baseIdents == null) throw new ArgumentNullException(nameof(baseIdents));

        var idents = baseIdents.ToList();

        var uniqueIdents = idents.Distinct().ToList();

        var uniqueResult = uniqueIdents.Split(MaxItemsInSql)
                                       .SelectMany(
                                                   path => this.GetUnsecureQueryable(fetchContainer)
                                                               .Where(v => path.Contains(v.Id))
                                                               .Select(projectionSelector))
                                       .ToList();

        if (uniqueResult.Count == uniqueIdents.Count)
        {
            var resultRequest = from ident in idents
                                join projection in uniqueResult on ident equals ((IIdentityObject<TIdent>)projection).Id
                                select projection;

            return resultRequest.ToList();
        }
        else
        {
            throw uniqueIdents.Except(uniqueResult.Select(v => ((IIdentityObject<TIdent>)v!).Id))
                              .Select(this.GetMissingObjectException)
                              .Aggregate();
        }
    }

    protected List<TDomainObject> GetListByIdentsNoSecurable(IEnumerable<TIdent> baseIdents, IFetchContainer<TDomainObject>? fetchContainer = null)
    {
        if (baseIdents == null)
        {
            throw new ArgumentNullException(nameof(baseIdents));
        }

        var idents = baseIdents.ToList();

        var uniqueIdents = idents.Distinct().ToList();

        var uniqueResult = uniqueIdents.Split(MaxItemsInSql).SelectMany(path => this.GetUnsecureQueryable(fetchContainer).Where(v => path.Contains(v.Id))).ToList();

        if (uniqueResult.Count == uniqueIdents.Count)
        {
            var resultRequest = from ident in idents
                                join domainObject in uniqueResult on ident equals domainObject.Id
                                select domainObject;

            return resultRequest.ToList();
        }

        throw uniqueIdents.Except(uniqueResult.Select(v => v.Id)).Select(this.GetMissingObjectException).Aggregate();
    }

    protected virtual Exception GetMissingObjectException(TIdent id)
    {
        return new ObjectByIdNotFoundException<TIdent>(typeof(TDomainObject), id);
    }

    private TDomainObject? GetNested<TNestedDomainObject>(TDomainObject domainObject)
            where TNestedDomainObject : class, TDomainObject =>
            this.Context.Logics.Default.Create<TNestedDomainObject>().GetById(domainObject.Id);
}
