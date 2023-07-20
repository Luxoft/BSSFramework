using System.Linq.Expressions;

using Framework.Core;
using Framework.DomainDriven.BLL.Security.Lock;
using Framework.HierarchicalExpand;
using Framework.Persistent;

namespace Framework.DomainDriven.BLL.Security;

public class SyncDenormolizedValuesService<TBLLContext, TPersistentDomainObjectBase, TDomainObject,
                                           TDomainObjectAncestorLink,
                                           TSourceToAncestorOrChildLink, TIdent, TNamedLockObject, TNamedLockOperation>
        where TBLLContext : class, IBLLBaseContextBase<TPersistentDomainObjectBase, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObjectAncestorLink : class, TPersistentDomainObjectBase, IModifiedHierarchicalAncestorLink<TDomainObject, TSourceToAncestorOrChildLink, TIdent>, new()
        where TDomainObject : class, TPersistentDomainObjectBase, IDenormalizedHierarchicalPersistentSource<TDomainObjectAncestorLink, TSourceToAncestorOrChildLink, TDomainObject, TIdent>
        where TIdent : struct, IComparable<TIdent>
        where TNamedLockObject : class, TPersistentDomainObjectBase, INamedLock<TNamedLockOperation>
        where TNamedLockOperation : struct, Enum
        where TSourceToAncestorOrChildLink : IHierarchicalToAncestorOrChildLink<TDomainObject, TIdent>
{
    private readonly IDAL<TDomainObject, Guid> domainObjectDal;

    private readonly IDAL<TDomainObjectAncestorLink, Guid> domainObjectAncestorLinkDal;

    private readonly IDAL<TNamedLockObject, Guid> namedLockObjectDal;

    private MemberExpression identityPropertyExpression;


    public SyncDenormolizedValuesService(
            IDAL<TDomainObject, Guid> domainObjectDal,
            IDAL<TDomainObjectAncestorLink, Guid> domainObjectAncestorLinkDal,
            IDAL<TNamedLockObject, Guid> namedLockObjectDal)
    {
        this.domainObjectDal = domainObjectDal;
        this.domainObjectAncestorLinkDal = domainObjectAncestorLinkDal;
        this.namedLockObjectDal = namedLockObjectDal;
        this.Init();
    }

    public void SyncUp(TDomainObject domainObject)
    {
        this.LockChanges();

        var ancestorDiffLinks = this.GetAncestorDiffirence(domainObject);

        Func<DiffAncestorLinks, MergeResult<TDomainObjectAncestorLink, AncestorLink>> getMergeFunc = diff =>
                diff.PersistentLinks.GetMergeResult(diff.ActualLinks,
                                                    z => new { Child = z.Child, Ancestor = z.Ancestor },
                                                    z => new { Child = z.Child, Ancestor = z.Ancestor },
                                                    (previus, current) =>
                                                            previus.Child.Id.CompareTo(current.Child.Id) == 0 &&
                                                            previus.Ancestor.Id.CompareTo(current.Ancestor.Id) == 0);


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

        foreach (var domainObject in this.domainObjectDal.GetQueryable(LockRole.None).ToList())
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

        var newAncestorLinks = combine.Adding.Select(z => new TDomainObjectAncestorLink { Ancestor = z.Ancestor, Child = z.Child }).ToList();

        newAncestorLinks.Foreach(this.SaveAncestor);

        combine.Removing.Foreach(this.RemoveAncestor);

        updatedDomainObjects.Foreach(this.UpdateDeepLevel);
    }

    private void UpdateDeepLevel(TDomainObject domainObject)
    {
        if (domainObject is IModifiedIHierarchicalLevelObject deepLevelObject)
        {
            deepLevelObject.DeepLevel = domainObject.GetAllParents(true).Count();
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
                                    .GetQueryable(LockRole.None)
                                    .Where(z => domainObjectIdents.Contains(z.Ancestor.Id) || domainObjectIdents.Contains(z.Child.Id)).ToList();

        return new SyncResult(new AncestorLink[0], removingAncestors);
    }
    private SyncResult GetSyncResult(TDomainObject domainObject)
    {
        this.LockChanges();

        var ancestorDiffs = domainObject.GetAllChildren().Select(this.GetAncestorDiffirence).ToList();

        Func<DiffAncestorLinks, MergeResult<TDomainObjectAncestorLink, AncestorLink>> getMergeFunc = diff =>
                diff.PersistentLinks.GetMergeResult(diff.ActualLinks,
                                                    z => new { Child = z.Child, Ancestor = z.Ancestor },
                                                    z => new { Child = z.Child, Ancestor = z.Ancestor },
                                                    (previus, current) => previus.Child.Id.CompareTo(current.Child.Id) == 0 && previus.Ancestor.Id.CompareTo(current.Ancestor.Id) == 0);

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
        var collectin = new[]
                        {
                                NamedLockExtension.GetGlobalLock<TNamedLockOperation>(
                                                                                      typeof(TDomainObjectAncestorLink))
                        };

        var queryable = this.namedLockObjectDal.GetQueryable(LockRole.None);

        var allNamed = queryable.Where(z => collectin.Contains(z.LockOperation)).ToList();

        var namedLock = allNamed.Single(
                                        () => new System.ArgumentException($"System must have namedLock for {typeof(TDomainObjectAncestorLink).Name} global lock "),
                                        () => new System.ArgumentException($"System have more then one namedLock for {typeof(TDomainObjectAncestorLink).Name} global lock "));

        this.namedLockObjectDal.Lock(namedLock, LockRole.Update);
    }

    /// <summary>
    /// The get ancestor diffirence.
    /// </summary>
    /// <param name="domainObject">
    /// The domain object.
    /// </param>
    /// <returns>
    /// The <see cref="SyncDenormolizedValuesService"/>.
    /// </returns>
    private DiffAncestorLinks GetAncestorDiffirence(TDomainObject domainObject)
    {
        if (domainObject == null)
        {
            throw new ArgumentNullException(nameof(domainObject));
        }

        var ancestorLogic = this.domainObjectAncestorLinkDal.GetQueryable(LockRole.None);

        var actualAncestors = domainObject.GetAllParents().Select(z => new AncestorLink(child: domainObject, ancestor: z)).ToList();

        var domainObjectIdents = new[] { domainObject.Id };

        var persistenAncestors = ancestorLogic.Where(z => domainObjectIdents.Contains(z.Child.Id)).ToList();

        return new DiffAncestorLinks(actualAncestors, persistenAncestors);
    }

    private void RemoveAncestor(TDomainObjectAncestorLink domainObjectAncestorLink)
    {
        this.domainObjectAncestorLinkDal.Remove(domainObjectAncestorLink);
    }

    private void SaveAncestor(TDomainObjectAncestorLink domainObjectAncestorLink)
    {
        this.domainObjectAncestorLinkDal.Save(domainObjectAncestorLink);
    }

    private Expression<Func<TDomainObject, bool>> GetEqualsIdentityExpression(TDomainObject domainObject)
    {
        var parameter = Expression.Parameter(typeof(TDomainObject), "z");
        var variable = Expression.Constant(domainObject);


        var domainObjectIdPropertyExpression = Expression.Property(variable, this.identityPropertyExpression.Member.Name);

        var currentParameterIdPropertyExpression = Expression.Property(parameter, this.identityPropertyExpression.Member.Name);

        var equalExpression = Expression.Equal(domainObjectIdPropertyExpression, currentParameterIdPropertyExpression);

        var result = Expression.Lambda<Func<TDomainObject, bool>>(equalExpression, parameter);

        return result;
    }

    enum UpdateAncestorLinksMode
    {
        None,
        CreateNew,
        UpdateExists
    }

    struct AncestorLink
    {
        public readonly TDomainObject Child;
        public readonly TDomainObject Ancestor;

        public AncestorLink(TDomainObject child, TDomainObject ancestor)
                : this()
        {
            this.Child = child;
            this.Ancestor = ancestor;
        }
    }

    struct DiffAncestorLinks
    {
        public readonly IEnumerable<AncestorLink> ActualLinks;
        public readonly IEnumerable<TDomainObjectAncestorLink> PersistentLinks;

        public DiffAncestorLinks(IEnumerable<AncestorLink> actualLinks, IEnumerable<TDomainObjectAncestorLink> persistentLinks)
                : this()
        {
            this.PersistentLinks = persistentLinks;
            this.ActualLinks = actualLinks;
        }
    }

    /// <summary>
    /// The sync result.
    /// </summary>
    struct SyncResult
    {
        /// <summary>
        /// The removing.
        /// </summary>
        public readonly IEnumerable<TDomainObjectAncestorLink> Removing;

        /// <summary>
        /// The adding.
        /// </summary>
        public readonly IEnumerable<AncestorLink> Adding;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncResult"/> struct.
        /// </summary>
        /// <param name="adding">
        /// The adding.
        /// </param>
        /// <param name="removing">
        /// The removing.
        /// </param>
        public SyncResult(IEnumerable<AncestorLink> adding, IEnumerable<TDomainObjectAncestorLink> removing)
                : this()
        {
            if (adding == null)
            {
                throw new ArgumentNullException(nameof(adding));
            }
            if (removing == null)
            {
                throw new ArgumentNullException(nameof(removing));
            }
            this.Adding = adding.ToList();
            this.Removing = removing.ToList();
        }

        /// <summary>
        /// The union.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The <see cref="SyncDenormolizedValuesService"/>.
        /// </returns>
        public SyncResult Union(SyncResult other)
        {
            return new SyncResult(this.Adding.Union(other.Adding), this.Removing.Union(other.Removing));
        }
    }
}
