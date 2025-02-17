using System.Linq.Expressions;

using Framework.Core;
using Framework.DomainDriven.Lock;
using Framework.HierarchicalExpand;
using Framework.Persistent;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class SyncDenormalizedValuesService<TDomainObject,
                                           TDomainObjectAncestorLink,
                                           TSourceToAncestorOrChildLink, TIdent>
    where TDomainObjectAncestorLink : class, IModifiedHierarchicalAncestorLink<TDomainObject, TSourceToAncestorOrChildLink, TIdent>, new()
    where TDomainObject : class, IDenormalizedHierarchicalPersistentSource<TDomainObjectAncestorLink, TSourceToAncestorOrChildLink,
    TDomainObject, TIdent>
    where TIdent : struct, IComparable<TIdent>
    where TSourceToAncestorOrChildLink : IHierarchicalToAncestorOrChildLink<TDomainObject, TIdent>
{
    private readonly IAsyncDal<TDomainObject, Guid> domainObjectDal;

    private readonly IAsyncDal<TDomainObjectAncestorLink, Guid> domainObjectAncestorLinkDal;

    private readonly INamedLockSource namedLockSource;

    private readonly INamedLockService namedLockService;

    private MemberExpression identityPropertyExpression;


    public SyncDenormalizedValuesService(
        IAsyncDal<TDomainObject, Guid> domainObjectDal,
        IAsyncDal<TDomainObjectAncestorLink, Guid> domainObjectAncestorLinkDal,
        INamedLockSource namedLockSource,
        INamedLockService namedLockService)
    {
        this.domainObjectDal = domainObjectDal;
        this.domainObjectAncestorLinkDal = domainObjectAncestorLinkDal;
        this.namedLockSource = namedLockSource;
        this.namedLockService = namedLockService;
        this.Init();
    }

    public void SyncUp(TDomainObject domainObject)
    {
        this.LockChanges();

        var ancestorDiffLinks = this.GetAncestorDifference(domainObject);

        Func<DiffAncestorLinks, MergeResult<TDomainObjectAncestorLink, AncestorLink>> getMergeFunc = diff =>
            diff.PersistentLinks.GetMergeResult(
                diff.ActualLinks,
                z => new { Child = z.Child, Ancestor = z.Ancestor },
                z => new { Child = z.Child, Ancestor = z.Ancestor },
                (previus, current) =>
                    previus.Child.Id.CompareTo(current.Child.Id) == 0 && previus.Ancestor.Id.CompareTo(current.Ancestor.Id) == 0);


        var ancestorMergeResult = getMergeFunc(ancestorDiffLinks);


        var addedLinks = ancestorMergeResult
                         .AddingItems
                         .Select(z => new TDomainObjectAncestorLink { Ancestor = z.Ancestor, Child = z.Child });


        ancestorMergeResult.RemovingItems.Foreach(this.RemoveAncestor);
        addedLinks.Foreach(this.SaveAncestor);
    }

    public void SyncAll()
    {
        this.LockChanges();

        foreach (var domainObject in this.domainObjectDal.GetQueryable().ToList())
        {
            this.SyncUp(domainObject);
        }
    }

    public void Sync(IEnumerable<TDomainObject> updatedDomainObjectsBase, IEnumerable<TDomainObject> removedDomainObjects)
    {
        this.LockChanges();

        var updatedDomainObjects = updatedDomainObjectsBase.ToList();

        var fromSyncResult = updatedDomainObjects.Select(this.GetSyncResult).ToList();

        var fromUnSyncResult = this.GetUnSyncResult(removedDomainObjects);

        var combine = fromSyncResult.Concat(new[] { fromUnSyncResult }).Aggregate((prev, current) => prev.Union(current));

        var newAncestorLinks = combine.Adding.Select(z => new TDomainObjectAncestorLink { Ancestor = z.Ancestor, Child = z.Child })
                                      .ToList();

        newAncestorLinks.Foreach(this.SaveAncestor);

        combine.Removing.Foreach(this.RemoveAncestor);

        updatedDomainObjects.Foreach(this.UpdateDeepLevel);
    }

    private void UpdateDeepLevel(TDomainObject domainObject)
    {
        if (domainObject is IHierarchicalLevelObjectDenormalized objectDenomalized)
        {
            objectDenomalized.SetDeepLevel(domainObject.GetAllParents(true).Count());
        }
    }

    private void Init()
    {
        Expression<Func<TDomainObject, TIdent>> idExpression = z => z.Id;
        this.identityPropertyExpression = (MemberExpression)idExpression.Body;
    }

    private SyncResult GetUnSyncResult(IEnumerable<TDomainObject> domainObjects)
    {
        if (domainObjects == null)
        {
            throw new ArgumentNullException(nameof(domainObjects));
        }

        this.LockChanges();

        var domainObjectIdents = domainObjects.Select(z => z.Id).ToList();

        var removingAncestors = this.domainObjectAncestorLinkDal
                                    .GetQueryable()
                                    .Where(z => domainObjectIdents.Contains(z.Ancestor.Id) || domainObjectIdents.Contains(z.Child.Id))
                                    .ToList();

        return new SyncResult(new AncestorLink[0], removingAncestors);
    }

    private SyncResult GetSyncResult(TDomainObject domainObject)
    {
        this.LockChanges();

        var ancestorDiffs = domainObject.GetAllChildren().Select(this.GetAncestorDifference).ToList();

        Func<DiffAncestorLinks, MergeResult<TDomainObjectAncestorLink, AncestorLink>> getMergeFunc = diff =>
            diff.PersistentLinks.GetMergeResult(
                diff.ActualLinks,
                z => new { Child = z.Child, Ancestor = z.Ancestor },
                z => new { Child = z.Child, Ancestor = z.Ancestor },
                (previus, current) => previus.Child.Id.CompareTo(current.Child.Id) == 0
                                      && previus.Ancestor.Id.CompareTo(current.Ancestor.Id) == 0);

        var mergeResults = ancestorDiffs.Select(getMergeFunc).ToList();

        if (!mergeResults.Any())
        {
            return new SyncResult();
        }

        var addedLinks = mergeResults.Any(q => q.AddingItems.Any())
                             ? mergeResults.Select(z => (IEnumerable<AncestorLink>)z.AddingItems)
                                           .Aggregate((prev, current) => prev.Concat(current))
                             : new AncestorLink[0];

        var removingLinks = mergeResults.Any(q => q.RemovingItems.Any())
                                ? mergeResults.Select(z => (IEnumerable<TDomainObjectAncestorLink>)z.RemovingItems)
                                              .Aggregate((prev, current) => prev.Concat(current))
                                : new TDomainObjectAncestorLink[0];


        return new SyncResult(addedLinks, removingLinks);
    }

    private void LockChanges()
    {
        var namedLock = this.namedLockSource.NamedLocks.Where(nl => nl.DomainType == typeof(TDomainObjectAncestorLink)).Single(
            () => new ArgumentException($"System must have namedLock for {typeof(TDomainObjectAncestorLink).Name} global lock "),
            () => new ArgumentException(
                $"System have more then one namedLock for {typeof(TDomainObjectAncestorLink).Name} global lock "));

        this.namedLockService.LockAsync(namedLock, LockRole.Update).GetAwaiter().GetResult();
    }

    /// <summary>
    /// The get ancestor diffirence.
    /// </summary>
    /// <param name="domainObject">
    /// The domain object.
    /// </param>
    /// <returns>
    /// The <see cref="SyncDenormalizedValuesService"/>.
    /// </returns>
    private DiffAncestorLinks GetAncestorDifference(TDomainObject domainObject)
    {
        if (domainObject == null)
        {
            throw new ArgumentNullException(nameof(domainObject));
        }

        var ancestorLogic = this.domainObjectAncestorLinkDal.GetQueryable();

        var actualAncestors = domainObject.GetAllParents().Select(z => new AncestorLink(Child: domainObject, Ancestor: z)).ToList();

        var domainObjectIdents = new[] { domainObject.Id };

        var persistenAncestors = ancestorLogic.Where(z => domainObjectIdents.Contains(z.Child.Id)).ToList();

        return new DiffAncestorLinks(actualAncestors, persistenAncestors);
    }

    private void RemoveAncestor(TDomainObjectAncestorLink domainObjectAncestorLink)
    {
        this.domainObjectAncestorLinkDal.RemoveAsync(domainObjectAncestorLink).GetAwaiter().GetResult();
    }

    private void SaveAncestor(TDomainObjectAncestorLink domainObjectAncestorLink)
    {
        this.domainObjectAncestorLinkDal.SaveAsync(domainObjectAncestorLink).GetAwaiter().GetResult();
    }

    private readonly record struct AncestorLink(TDomainObject Child, TDomainObject Ancestor);

    private readonly record struct DiffAncestorLinks(
        IEnumerable<AncestorLink> ActualLinks,
        IEnumerable<TDomainObjectAncestorLink> PersistentLinks);

    private readonly record struct SyncResult(IEnumerable<AncestorLink> Adding, IEnumerable<TDomainObjectAncestorLink> Removing)
    {
        public SyncResult Union(SyncResult other)
        {
            return new SyncResult(this.Adding.Union(other.Adding), this.Removing.Union(other.Removing));
        }
    }
}
