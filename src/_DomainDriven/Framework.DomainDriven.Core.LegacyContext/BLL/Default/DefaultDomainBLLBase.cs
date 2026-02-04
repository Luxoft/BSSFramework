using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using CommonFramework;
using Framework.Core;
using Framework.DomainDriven.Lock;
using Framework.Exceptions;
using Framework.OData;
using Framework.Persistent;
using GenericQueryable;
using GenericQueryable.Fetching;
using HierarchicalExpand;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.BLL;

public abstract class DefaultDomainBLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TIdent>(TBLLContext context) :
    BLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TIdent>(context),
    IDefaultDomainBLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TIdent>
    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    where TDomainObject : class, TPersistentDomainObjectBase
    where TBLLContext : class, IDefaultBLLContext<TPersistentDomainObjectBase, TIdent>, IHierarchicalObjectExpanderFactoryContainer,
    IBLLBaseContext
    where TIdent : notnull
{
    private const int MaxItemsInSql = 2000;

    public TDomainObject? GetById(TIdent id, IdCheckMode idCheckMode, FetchRule<TDomainObject>? fetchRule = null, LockRole lockRole = LockRole.None) =>
            idCheckMode switch
            {
                    IdCheckMode.DontCheck => this.GetById(id, false, fetchRule, lockRole),
                    IdCheckMode.CheckAll => this.GetById(id, true, fetchRule, lockRole),
                    IdCheckMode.SkipEmpty => id.IsDefault() ? null : this.GetById(id, true, fetchRule, lockRole),
                    _ => throw new ArgumentOutOfRangeException(nameof(idCheckMode))
            };

    public TDomainObject? GetById(TIdent id, IdCheckMode idCheckMode, Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule) =>
        this.GetById(id, idCheckMode, buildFetchRule.ToFetchRule());

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
    public virtual List<HierarchicalNode<TDomainObject, TIdent>> GetTree(FetchRule<TDomainObject>? fetchRule = null) =>
            this.GetTree(this.GetFullList(fetchRule), _ => this.GetSecureQueryable().Select(domainObject => domainObject.Id), this.ExpandQueryableWithParents);

    /// <inheritdoc />
    public virtual SelectOperationResult<HierarchicalNode<TDomainObject, TIdent>> GetTreeByOData(
            SelectOperation<TDomainObject> selectOperation,
            FetchRule<TDomainObject>? fetchRule = null)
    {
        if (selectOperation == null)
        {
            throw new ArgumentNullException(nameof(selectOperation));
        }

        var odataResult = this.GetObjectsByOData(selectOperation, fetchRule);

        var tree = this.GetTree(odataResult.Items, x => x.ToList(domainObject => domainObject.Id), this.ExpandEnumerableWithParents);

        return new SelectOperationResult<HierarchicalNode<TDomainObject, TIdent>>(tree, tree.Count);
    }

    private List<HierarchicalNode<TDomainObject, TIdent>> GetTree<TIdentCollection>(
            List<TDomainObject> startProjections,
            Func<IEnumerable<TDomainObject>, TIdentCollection> identsSelector,
            Func<TIdentCollection, HierarchicalExpandType, Dictionary<TIdent, TIdent>> parentsExpander,
            FetchRule<TDomainObject>? fetchRule = null)
            where TIdentCollection : IEnumerable<TIdent>
    {
        if (startProjections == null)
        {
            throw new ArgumentNullException(nameof(startProjections));
        }

        var projectionsIdents = identsSelector(startProjections);

        var allowedExpandTreeParents = this.Context.ServiceProvider.GetService<ExpandTreeInfo<TDomainObject>>()?.ParentsExpandAllowed ?? true;

        var parentExpandMode = allowedExpandTreeParents ? HierarchicalExpandType.Parents : HierarchicalExpandType.None;

        var allIdentDict = parentsExpander(projectionsIdents, parentExpandMode);

        var expandedParents = allIdentDict.Keys.Except(projectionsIdents).ToList();

        var parents = this.Context.Logics.Default.Create<TDomainObject>().GetListByIdents(expandedParents, fetchRule);

        return startProjections.Select(item => new { Item = item, OnlyView = false })
                               .Concat(parents.Select(item => new { Item = item, OnlyView = true }))
                               .ToList(
                                   pair => new HierarchicalNode<TDomainObject, TIdent>(

                                       Item: pair.Item,
                                       OnlyView: pair.OnlyView,
                                       ParentId: allIdentDict[pair.Item.Id]
                                       ));
    }

    private Dictionary<TIdent, TIdent> ExpandEnumerableWithParents(IEnumerable<TIdent> projectionsIdents, HierarchicalExpandType parentExpandMode)
    {
        var hierarchicalObjectExpander = this.Context.HierarchicalObjectExpanderFactory.Create<TIdent>(typeof(TDomainObject));

        return projectionsIdents.Split(MaxItemsInSql)
                                .SelectMany(z => hierarchicalObjectExpander.ExpandWithParents(z, parentExpandMode))
                                .Distinct()
                                .ToDictionary(z => z.Key, z => z.Value);
    }

    private Dictionary<TIdent, TIdent> ExpandQueryableWithParents(IQueryable<TIdent> projectionsIdents, HierarchicalExpandType parentExpandMode)
    {
        return this.Context.HierarchicalObjectExpanderFactory.Create<TIdent>(typeof(TDomainObject)).ExpandWithParents(projectionsIdents, parentExpandMode);
    }

    public List<TDomainObject> GetListByIdents<TIdentity>(IEnumerable<TIdentity> idents, FetchRule<TDomainObject>? fetchRule = null)
            where TIdentity : IIdentityObject<TIdent>
    {
        return this.GetListByIdents(idents.Select(ident => ident.Id), fetchRule);
    }

    public List<TDomainObject> GetListByIdents(IEnumerable<TIdent> baseIdents, FetchRule<TDomainObject>? fetchRule = null)
    {
        if (baseIdents == null)
        {
            throw new ArgumentNullException(nameof(baseIdents));
        }

        var idents = baseIdents.ToList();

        var uniqueIdents = idents.Distinct().ToList();

        var uniqueResult = uniqueIdents.Split(MaxItemsInSql).SelectMany(path => this.GetListBy(v => path.Contains(v.Id), fetchRule)).ToList();

        if (uniqueResult.Count == uniqueIdents.Count)
        {
            var resultRequest = from ident in idents
                                join domainObject in uniqueResult on ident equals domainObject.Id
                                select domainObject;

            return resultRequest.ToList();
        }

        throw uniqueIdents.Except(uniqueResult.Select(v => v.Id)).Select(this.GetMissingObjectException).Aggregate();
    }

    public List<TDomainObject> GetListByIdents(IEnumerable<TIdent> baseIdents, Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule) =>
        this.GetListByIdents(baseIdents, buildFetchRule.ToFetchRule());

    public override SelectOperationResult<TDomainObject> GetObjectsByOData(
            SelectOperation selectOperation,
            FetchRule<TDomainObject>? fetchRule = null)
    {
        var typedSelectOperation = this.Context.StandartExpressionBuilder.ToTyped<TDomainObject>(selectOperation);

        return this.GetObjectsByOData(typedSelectOperation, fetchRule);
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
            FetchRule<TDomainObject>? fetchRule = null)
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

            var result = this.GetListByIdentsNoSecurable(idents, fetchRule);

            var count = countQuery.Count();

            return new SelectOperationResult<TDomainObject>(result, count);
        }
        else
        {
            var result = this.GetSecureQueryable(selectOperation, fetchRule);

            return new SelectOperationResult<TDomainObject>(result);
        }
    }

    public List<TDomainObject> GetListByIdentsUnsafe(IEnumerable<TIdent> baseIdents, FetchRule<TDomainObject>? fetchRule = null)
    {
        if (baseIdents == null)
        {
            throw new ArgumentNullException(nameof(baseIdents));
        }

        return baseIdents.Distinct().Split(MaxItemsInSql).SelectMany(path => this.GetListBy(v => path.Contains(v.Id), fetchRule)).ToList();
    }

    public List<TDomainObject> GetListByIdentsUnsafe(IEnumerable<TIdent> baseIdents, Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule) =>
        this.GetListByIdentsUnsafe(baseIdents, buildFetchRule.ToFetchRule());

    public TDomainObject? GetById(TIdent id, bool throwOnNotFound = false, FetchRule<TDomainObject>? fetchRule = null, LockRole lockRole = LockRole.None)
    {
        var result = this.GetListBy(domainObject => domainObject.Id.Equals(id), fetchRule, lockRole).FirstOrDefault();

        if (result == null && throwOnNotFound)
        {
            throw this.GetMissingObjectException(id);
        }

        return result;
    }

    public TDomainObject? GetById(TIdent id, bool throwOnNotFound, Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule) =>
        this.GetById(id, throwOnNotFound, buildFetchRule.ToFetchRule());

    protected List<TDomainObject> GetListByIdentsQueryable(IQueryable<TIdent> baseIdents, FetchRule<TDomainObject>? fetchRule = null)
    {
        if (baseIdents == null)
        {
            throw new ArgumentNullException(nameof(baseIdents));
        }

        var result = this.GetListBy(v => baseIdents.Contains(v.Id), fetchRule).ToList();

        return result;
    }

    protected List<TProjection> GetListByIdentsNoSecurable<TProjection>(
            IEnumerable<TIdent> baseIdents,
            Expression<Func<TDomainObject, TProjection>> projectionSelector,
            FetchRule<TDomainObject>? fetchRule = null)
    {
        if (baseIdents == null) throw new ArgumentNullException(nameof(baseIdents));

        var idents = baseIdents.ToList();

        var uniqueIdents = idents.Distinct().ToList();

        var uniqueResult = uniqueIdents.Split(MaxItemsInSql)
                                       .SelectMany(
                                                   path => this.GetUnsecureQueryable(fetchRule)
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

    protected List<TDomainObject> GetListByIdentsNoSecurable(IEnumerable<TIdent> baseIdents, FetchRule<TDomainObject>? fetchRule = null)
    {
        if (baseIdents == null)
        {
            throw new ArgumentNullException(nameof(baseIdents));
        }

        var idents = baseIdents.ToList();

        var uniqueIdents = idents.Distinct().ToList();

        var uniqueResult = uniqueIdents.Split(MaxItemsInSql).SelectMany(path => this.GetUnsecureQueryable(fetchRule).Where(v => path.Contains(v.Id))).ToList();

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
