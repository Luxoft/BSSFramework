using System.Linq.Expressions;

namespace Framework.Database.EntityFramework.EnversAudit;

public interface IEfAuditReader
{
    TEntity Find<TEntity>(object id, long revision)
        where TEntity : class;

    IReadOnlyList<TEntity> FindObjects<TEntity>(IEnumerable<object> ids, long revision)
        where TEntity : class;

    IReadOnlyList<long> GetRevisions(Type entityType, object id);

    IReadOnlyList<long> GetRevisions(Type entityType, object id, long maxRevision);

    long? GetPreviousRevision(Type entityType, object id, long maxRevision);

    long GetCurrentRevision();

    long GetMaxRevision();

    IReadOnlyList<Tuple<TEntity, long>> GetDomainObjectRevisions<TEntity>(object id, int takeCount)
        where TEntity : class;

    IReadOnlyList<AuditRevisionInfo> GetObjectRevisions(Type entityType, object id, DateTime? fromDate = null, DateTime? toDate = null);

    IReadOnlyList<AuditPropertyRevisionInfo<TProperty>> GetPropertyRevisions<TProperty>(Type entityType, object id, string propertyName, DateTime? fromDate = null, DateTime? toDate = null);

    IReadOnlyList<TIdent> GetIdentiesWithHistory<TEntity, TIdent>(Expression<Func<TEntity, bool>> predicate)
        where TEntity : class;
}
