using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.DAL.Revisions;
using Framework.DomainDriven.Lock;
using Framework.Persistent;

namespace Framework.DomainDriven.UnitTest.Mock;

public class MockDAL<TDomain, TIdent> : IMockDAL<TDomain, TIdent>
    where TDomain : IIdentityObject<TIdent>
{
    private readonly HashSet<TDomain> _collection = new HashSet<TDomain>();

    private readonly IList<Action<HashSet<TDomain>>> _actions = new List<Action<HashSet<TDomain>>>();

    public MockDAL(IList<TDomain> collection)
    {
        this._collection = new HashSet<TDomain>(collection);
    }

    public MockDAL()
    {
    }

    public MockDAL(TDomain value) : this(new[] { value })
    {
    }

    protected HashSet<TDomain> Collection
    {
        get { return this._collection; }
    }

    protected IList<Action<HashSet<TDomain>>> Actions
    {
        get { return this._actions; }
    }

    public TDomain GetObjectByRevision(TIdent id, long revisionNumber)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TDomain> GetObjectsByRevision(IEnumerable<TIdent> id, long revisionNumber)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<long> GetRevisions(TIdent id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<long> GetRevisions(TIdent id, long maxRevision)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<Tuple<T, long>> GetDomainObjectRevisions<T>(TIdent id, int takeCount) where T : class
    {
        throw new NotImplementedException();
    }

    public long? GetPreviousRevision(TIdent id, long maxRevision)
    {
        throw new NotImplementedException();
    }

    public long GetCurrentRevision()
    {
        throw new NotImplementedException();
    }

    public DomainObjectPropertyRevisions<TIdent, TProperty> GetPropertyRevisions<TProperty>(TIdent id, Expression<Func<TDomain, TProperty>> propertyExpression, Period? period = null)
    {
        throw new NotImplementedException();
    }

    public DomainObjectPropertyRevisions<TIdent, TProperty> GetPropertyRevisions<TProperty>(TIdent id, string propertyName, Period? period = null)
    {
        throw new NotImplementedException();
    }

    public IDomainObjectPropertyRevisionBase<TIdent, RevisionInfoBase> GetUntypedPropertyRevisions(TIdent id, string propertyName, Period? period = null)
    {
        throw new NotImplementedException();
    }

    public DomainObjectRevision<TIdent> GetObjectRevisions(TIdent identity, Period? period = null)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TIdent> GetIdentiesWithHistory(Expression<Func<TDomain, bool>> query)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<TIdent> GetIdentiesWithHistory(Expression<Func<TDomain, bool>> query, Period? period = null)
    {
        throw new NotImplementedException();
    }

    public void Refresh(TDomain domainObject)
    {
    }

    public void Lock(TDomain domainObject, LockRole lockRole)
    {
    }

    public IQueryable<TDomain> GetQueryable(LockRole lockRole, IFetchContainer<TDomain> fetchContainer = null)
    {
        var list = new List<TDomain>(this.Collection.Count);
        list.AddRange(this.Collection);
        return list.AsQueryable();
    }

    public virtual void Insert(TDomain domainObject, TIdent id)
    {
        SetIdentity(domainObject, id);
        this.Actions.Add(z => z.Add(domainObject));
    }

    public TDomain GetById(TIdent id, LockRole lockRole)
    {
        return this._collection.FirstOrDefault(z => EqualityComparer<TIdent>.Default.Equals(z.Id, id));
    }

    public IQueryable<TDomain> GetQueryable() => this.GetQueryable(LockRole.None);

    public TDomain Load(TIdent id)
    {
        return this.GetById(id, LockRole.None);
    }

    public async Task<TDomain> LoadAsync(TIdent id, CancellationToken cancellationToken = default)
    {
        return this.Load(id);
    }

    public async Task RefreshAsync(TDomain domainObject, CancellationToken cancellationToken = default)
    {
        this.Refresh(domainObject);
    }

    public async Task SaveAsync(TDomain domainObject, CancellationToken cancellationToken = default)
    {
        this.Save(domainObject);
    }

    public async Task InsertAsync(TDomain domainObject, TIdent id, CancellationToken cancellationToken = default)
    {
        this.Insert(domainObject, id);
    }

    public async Task RemoveAsync(TDomain domainObject, CancellationToken cancellationToken = default)
    {
        this.Remove(domainObject);
    }

    public async Task LockAsync(TDomain domainObject, LockRole lockRole, CancellationToken cancellationToken = default)
    {
        this.Lock(domainObject, lockRole);
    }

    public virtual void Save(TDomain domainObject)
    {
        InitIdentity(domainObject);
        this.Actions.Add(z =>
                         {
                             z.Add(domainObject);
                         });
    }

    public virtual void Remove(TDomain domainObject)
    {
        this.Actions.Add(z => z.Remove(domainObject));
    }

    public void Register(object value)
    {
        var domain = (TDomain)value;
        InitIdentity(domain);
        this.Collection.Add(domain);
    }

    public void Flush()
    {
        this.Actions.Foreach(z => z(this.Collection));
        this.Actions.Clear();
    }

    private static void InitIdentity(TDomain domainObject)
    {
        if (domainObject.Id.IsDefault() && typeof(TIdent) == typeof(Guid))
        {
            SetIdentity(domainObject, Guid.NewGuid());
        }
    }

    private static void SetIdentity(TDomain domainObject, object id)
    {
        typeof(TDomain).GetProperty(nameof(domainObject.Id), BindingFlags.Public | BindingFlags.Instance)
                       .SetValue(domainObject, id, new object[0]);
    }
}
