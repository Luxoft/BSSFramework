using System.Collections;
using System.Reflection;
using NHibernate.Engine;
using NHibernate.Envers.Configuration;
using NHibernate.Envers.Entities;
using NHibernate.Envers.Entities.Mapper.Id;
using NHibernate.Envers.Exceptions;
using NHibernate.Envers.Query;
using NHibernate.Envers.Query.Criteria;
using NHibernate.Envers.Reader;
using NHibernate.Envers.Tools;

namespace NHibernate.Envers.Patch;

public class AuditReaderPatched : AuditReader, IAuditReaderPatched
{
    private readonly AuditConfiguration verCfg;

    public AuditReaderPatched(AuditConfiguration verCfg, ISession session, ISessionImplementor sessionImplementor)
            : base(verCfg, session, sessionImplementor)
    {
        this.verCfg = verCfg;
    }

    public bool IsEmpty => false;

    public IList<T> FindObjects<T>(IEnumerable<object> primaryKeys, long revision)
    {
        return primaryKeys.Select(z => this.Find<T>(z, revision)).Where(z => null != z).ToList();
    }

    public IEnumerable<TIdentity> GetIdentiesBy<TDomainObject, TIdentity>(IAuditCriterion criterion)
    {
        var mapper = this.verCfg.EntCfg[typeof(TDomainObject).FullName].IdMapper;

        var idPropertyData = this.GetIdPropertyData(mapper);

        var idPropertyName = idPropertyData.Name.ToLower();

        var auditQuery = this.CreateQuery()

                             .ForRevisionsOfEntity(typeof(TDomainObject), true, false)
                             .Add(criterion);

        var result = auditQuery.AddProjection(AuditEntity.Property(idPropertyName)).GetResultList();

        return result
               .Cast<IDictionary>()
               .Select(z => z[idPropertyName])
               .Cast<TIdentity>()
               .Distinct()
               .ToList();
    }

    /// <summary>
    /// modify by podzyuban
    /// </summary>
    public IEnumerable<long> GetRevisions(System.Type cls, object primaryKey, long maxRevisions)
    {
        ArgumentsTools.CheckNotNull(primaryKey, "Primary key");
        var entityName = cls.FullName;

        if (!this.verCfg.EntCfg.IsVersioned(entityName))
        {
            throw new NotAuditedException(entityName, entityName + " is not versioned!");
        }

        var resultList = this.CreateQuery().ForRevisionsOfEntity(entityName, false, true)
                             .AddProjection(AuditEntity.RevisionNumber())
                             .Add(AuditEntity.Id().Eq(primaryKey))
                             .Add(AuditEntity.RevisionNumber().Lt(maxRevisions))
                             .GetResultList();

        return from object revision in resultList select Convert.ToInt64(revision);
    }

    public IList<Tuple<T, long>> GetDomainObjectRevisions<T>(object primaryKey, int takeCount)
            where T : class
    {
        ArgumentsTools.CheckNotNull(primaryKey, "Primary key");

        var entityName = typeof(T).FullName;

        if (!this.verCfg.EntCfg.IsVersioned(entityName))
        {
            throw new NotAuditedException(entityName, entityName + " is not versioned!");
        }

        var auditDomainObjects = this.CreateQuery()
                                     .ForRevisionsOfEntity(entityName, false, true)
                                     .Add(AuditEntity.Id().Eq(primaryKey))
                                     .AddOrder(AuditEntity.RevisionNumber().Desc())
                                     .SetMaxResults(takeCount)
                                     .GetResultList();

        var resultList = from object[] revisionoObject in auditDomainObjects
                         where revisionoObject[0] is T && revisionoObject[1].GetType().FullName.Contains("AuditRevisionEntity")
                         let revisionNumber = (long)revisionoObject[1].GetType().GetProperty("Id", typeof(long)).GetValue(revisionoObject[1], new object[] { })
                         select new Tuple<T, long>(revisionoObject[0] as T, revisionNumber);

        return resultList.ToList();
    }

    public long? GetPreviusRevision(System.Type cls, object primaryKey, long maxRevisions)
    {
        ArgumentsTools.CheckNotNull(primaryKey, "Primary key");
        var entityName = cls.FullName;

        if (!this.verCfg.EntCfg.IsVersioned(entityName))
        {
            throw new NotAuditedException(entityName, entityName + " is not versioned!");
        }

        var resultList = this.CreateQuery().ForRevisionsOfEntity(entityName, false, true)
                             .AddProjection(AuditEntity.RevisionNumber())
                             .Add(AuditEntity.Id().Eq(primaryKey))
                             .Add(AuditEntity.RevisionNumber().Lt(maxRevisions))
                             .AddOrder(AuditEntity.RevisionNumber().Desc())
                             .SetMaxResults(1)
                             .GetResultList();

        if (resultList.Cast<object>().Any())
        {
            return Convert.ToInt64(resultList.Cast<object>().First());
        }

        return null;
    }

    public AuditQueryCreatorPatched CreatePatchedQuery()
    {
        return new AuditQueryCreatorPatched(this.verCfg, this);
    }

    /// <summary>
    /// Gets the maximum audit revision.
    /// </summary>
    /// <returns>System.Int64.</returns>
    public long GetMaxRevision()
    {
        var result = this.CreatePatchedQuery()
                         .CreateRevisionEntityQuery()
                         .AddProjection(new AuditProperty(new RevisionNumberFieldPropertyName()).Max())
                         .GetSingleResult<long>();

        return result;
    }

    private PropertyData GetIdPropertyData(IIdMapper mapper)
    {
        var allFields = mapper.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
        var idFileInfo = allFields.Single(z => z.FieldType == typeof(PropertyData));
        return (PropertyData)idFileInfo.GetValue(mapper);
    }
}
