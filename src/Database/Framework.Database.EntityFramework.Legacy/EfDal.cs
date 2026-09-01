using System.Linq.Expressions;

using Anch.Core;
using Anch.GenericQueryable;
using Anch.GenericQueryable.Fetching;

using Framework.Core;
using Framework.Database.Domain;

namespace Framework.Database.EntityFramework;

public class EfDal<TDomainObject, TIdent>(IAsyncDal<TDomainObject, TIdent> asyncDal, IDefaultCancellationTokenSource? defaultCancellationTokenSource = null) : IDAL<TDomainObject, TIdent>
    where TDomainObject : class
    where TIdent : notnull
{
    public TDomainObject GetById(TIdent id, LockRole lockRole)
    {
        var domainObject = asyncDal.Load(id);

        if (lockRole != LockRole.None)
        {
            this.Lock(domainObject, lockRole);
        }

        return domainObject;
    }

    public void Lock(TDomainObject domainObject, LockRole lockRole) => defaultCancellationTokenSource.RunSync(ct => asyncDal.LockAsync(domainObject, lockRole, ct));

    public void Refresh(TDomainObject domainObject) => defaultCancellationTokenSource.RunSync(ct => asyncDal.RefreshAsync(domainObject, ct));

    public virtual void Save(TDomainObject domainObject) => defaultCancellationTokenSource.RunSync(ct => asyncDal.SaveAsync(domainObject, ct));

    public virtual void Insert(TDomainObject domainObject, TIdent id) => defaultCancellationTokenSource.RunSync(ct => asyncDal.InsertAsync(domainObject, id, ct));

    public virtual void Remove(TDomainObject domainObject) => defaultCancellationTokenSource.RunSync(ct => asyncDal.RemoveAsync(domainObject, ct));

    public IQueryable<TDomainObject> GetQueryable(LockRole lockRole, FetchRule<TDomainObject>? fetchRule = null)
    {
        var queryable = asyncDal.GetQueryable();

        var withFetchQueryable = fetchRule is null ? queryable : queryable.WithFetch(fetchRule);

        if (lockRole != LockRole.None)
        {
            throw new NotSupportedException("EF backend doesn't support queryable-level locking. Use GetById(id, lockRole) instead.");
        }

        return withFetchQueryable;
    }

    public TDomainObject Load(TIdent id) => asyncDal.Load(id);

    public TDomainObject GetObjectByRevision(TIdent id, long revision) => throw new NotImplementedException("EF");

    public IEnumerable<TDomainObject> GetObjectsByRevision(IEnumerable<TIdent> idCollection, long revisionNumber) => throw new NotImplementedException("EF");

    public IEnumerable<long> GetRevisions(TIdent id) => throw new NotImplementedException("EF");

    public IReadOnlyList<Tuple<T, long>> GetDomainObjectRevisions<T>(TIdent id, int takeCount)
        where T : class =>
        throw new NotImplementedException("EF");

    public IEnumerable<long> GetRevisions(TIdent id, long maxRevision) => throw new NotImplementedException("EF");

    public long? GetPreviousRevision(TIdent id, long maxRevision) => throw new NotImplementedException("EF");

    public long GetCurrentRevision() => throw new NotImplementedException("EF");

    public DomainObjectPropertyRevisions<TIdent, TProperty> GetPropertyRevisions<TProperty>(
        TIdent id,
        string propertyName,
        Period? period = null) =>
        throw new NotImplementedException("EF");

    public IDomainObjectPropertyRevisionBase<TIdent, RevisionInfoBase> GetUntypedPropertyRevisions(
        TIdent id,
        string propertyName,
        Period? period = null) =>
        throw new NotImplementedException("EF");

    public DomainObjectPropertyRevisions<TIdent, TProperty> GetPropertyRevisions<TProperty>(
        TIdent id,
        Expression<Func<TDomainObject, TProperty>> propertyExpression,
        Period? period = null) =>
        throw new NotImplementedException("EF");

    public DomainObjectRevision<TIdent> GetObjectRevisions(TIdent identity, Period? period = null) => throw new NotImplementedException("EF");

    public IEnumerable<TIdent> GetIdentiesWithHistory(Expression<Func<TDomainObject, bool>> query) => throw new NotImplementedException("EF");
}
