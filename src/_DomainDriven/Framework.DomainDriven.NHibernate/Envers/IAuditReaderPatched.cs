using NHibernate.Envers.Query.Criteria;

namespace NHibernate.Envers.Patch;

public interface IAuditReaderPatched : IAuditReader
{
    IEnumerable<TIdentity> GetIdentsBy<TDomainObject, TIdentity>(IAuditCriterion criterion);

    IReadOnlyList<T> FindObjects<T>(IEnumerable<object> primaryKeys, long revision);

    /// <summary>
    /// Get a list of revision numbers, at which an entity was modified.
    /// </summary>
    /// <param name="cls">Type of entity</param>
    /// <param name="primaryKey">Primary key of the entity.</param>
    /// <param name="maxRevisions">Not include</param>
    /// <returns></returns>
    IEnumerable<long> GetRevisions(System.Type cls, object primaryKey, long maxRevisions);

    IReadOnlyList<Tuple<T, long>> GetDomainObjectRevisions<T>(object primaryKey, int takeCount)
            where T : class;

    long? GetPreviusRevision(System.Type cls, object primaryKey, long maxRevisions);

    AuditQueryCreatorPatched CreatePatchedQuery();

    /// <summary>
    /// Gets the maximum audit revision.
    /// </summary>
    /// <returns>System.Int64.</returns>
    long GetMaxRevision();
}
