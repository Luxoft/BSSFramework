using NHibernate;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;

namespace Framework.DomainDriven.NHibernate
{
    internal static class SessionExtensions
    {
        public static IEntityPersister GetPersister(this ISession session, EntityEntry entityEntry)
        {
            var sessionImpl = session.GetSessionImplementation();

            var className = entityEntry.EntityName;

            return sessionImpl.Factory.GetEntityPersister(className);
        }

        public static EntityEntry GetEntityEntry(this ISession session, object entity)
        {
            var sessionImpl = session.GetSessionImplementation();

            var unProxy = sessionImpl.TryUnProxy(entity);

            var persistenceContext = sessionImpl.PersistenceContext;

            return persistenceContext.GetEntry(unProxy);
        }


        public static object TryUnProxy(this ISessionImplementor sessionImpl, object entity)
        {
            if (!NHibernateUtil.IsInitialized(entity))
            {
                NHibernateUtil.Initialize(entity);
            }

            if (entity is INHibernateProxy)
            {
                return sessionImpl.PersistenceContext.Unproxy(entity);
            }

            return entity;
        }
    }
}