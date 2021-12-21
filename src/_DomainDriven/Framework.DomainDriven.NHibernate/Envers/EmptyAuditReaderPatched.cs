using System;
using System.Collections.Generic;
using NHibernate.Envers.Query;
using NHibernate.Envers.Query.Criteria;

namespace NHibernate.Envers.Patch
{
    public class EmptyAuditReaderPatched : IAuditReaderPatched
    {
        public static readonly EmptyAuditReaderPatched Value = new EmptyAuditReaderPatched();

        public bool IsEmpty => true;

        public T Find<T>(object primaryKey, long revision)
        {
            throw new NotImplementedException(typeof(IAuditReaderPatched).Name + " is not initialized");
        }

        public object Find(System.Type cls, object primaryKey, long revision)
        {
            throw new NotImplementedException(typeof(IAuditReaderPatched).Name + " is not initialized");
        }

        public object Find(string entityName, object primaryKey, long revision)
        {
            throw new NotImplementedException(typeof(IAuditReaderPatched).Name + " is not initialized");
        }

        public object Find(string entityName, object primaryKey, long revision, bool includeDeletions)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<long> GetRevisions(System.Type cls, object primaryKey)
        {
            throw new NotImplementedException(typeof(IAuditReaderPatched).Name + " is not initialized");
        }

        public IEnumerable<long> GetRevisions(string entityName, object primaryKey)
        {
            throw new NotImplementedException(typeof(IAuditReaderPatched).Name + " is not initialized");
        }

        public DateTime GetRevisionDate(long revision)
        {
            throw new NotImplementedException(typeof(IAuditReaderPatched).Name + " is not initialized");
        }

        public long GetRevisionNumberForDate(DateTime date)
        {
            throw new NotImplementedException(typeof(IAuditReaderPatched).Name + " is not initialized");
        }

        public T FindRevision<T>(long revision)
        {
            throw new NotImplementedException(typeof(IAuditReaderPatched).Name + " is not initialized");
        }

        public object FindRevision(long revision)
        {
            throw new NotImplementedException(typeof(IAuditReaderPatched).Name + " is not initialized");
        }

        public IDictionary<long, object> FindRevisions(IEnumerable<long> revisions)
        {
            throw new NotImplementedException(typeof(IAuditReaderPatched).Name + " is not initialized");
        }

        public IDictionary<long, T> FindRevisions<T>(IEnumerable<long> revisions)
        {
            throw new NotImplementedException(typeof(IAuditReaderPatched).Name + " is not initialized");
        }

        public T GetCurrentRevision<T>(bool persist)
        {
            throw new NotImplementedException(typeof(IAuditReaderPatched).Name + " is not initialized");
        }

        public object GetCurrentRevision(bool persist)
        {
            throw new NotImplementedException(typeof(IAuditReaderPatched).Name + " is not initialized");
        }

        public AuditQueryCreator CreateQuery()
        {
            throw new NotImplementedException(typeof(IAuditReaderPatched).Name + " is not initialized");
        }

        public ICrossTypeRevisionChangesReader CrossTypeRevisionChangesReader()
        {
            throw new NotImplementedException(typeof(IAuditReaderPatched).Name + " is not initialized");
        }

        public string GetEntityName(object primaryKey, long revision, object entity)
        {
            throw new NotImplementedException(typeof(IAuditReaderPatched).Name + " is not initialized");
        }

        public bool IsEntityClassAudited(System.Type entityClass)
        {
            throw new NotImplementedException(typeof(IAuditReaderPatched).Name + " is not initialized");
        }

        public bool IsEntityNameAudited(string entityName)
        {
            throw new NotImplementedException(typeof(IAuditReaderPatched).Name + " is not initialized");
        }

        public IEnumerable<TIdentity> GetIdentiesBy<TDomainObject, TIdentity>(IAuditCriterion criterion)
        {
            throw new NotImplementedException(typeof(IAuditReaderPatched).Name + " is not initialized");
        }

        public IList<T> FindObjects<T>(IEnumerable<object> primaryKeys, long revision)
        {
            throw new NotImplementedException(typeof(IAuditReaderPatched).Name + " is not initialized");
        }

        public IEnumerable<long> GetRevisions(System.Type cls, object primaryKey, long maxRevisions)
        {
            throw new NotImplementedException(typeof(IAuditReaderPatched).Name + " is not initialized");
        }

        public IList<Tuple<T, long>> GetDomainObjectRevisions<T>(object primaryKey, int takeCount)
            where T : class
        {
            throw new NotImplementedException();
        }

        public long? GetPreviusRevision(System.Type cls, object primaryKey, long maxRevisions)
        {
            throw new NotImplementedException(typeof(IAuditReaderPatched).Name + " is not initialized");
        }

        public AuditQueryCreatorPatched CreatePatchedQuery()
        {
            throw new NotImplementedException(typeof(IAuditReaderPatched).Name + " is not initialized");
        }

        public long GetMaxRevision()
        {
            throw new NotImplementedException(typeof(IAuditReaderPatched).Name + " is not initialized");
        }
    }
}
