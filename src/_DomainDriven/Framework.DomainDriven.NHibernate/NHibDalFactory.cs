using System;

using Framework.DomainDriven.BLL;
using Framework.Persistent;

namespace Framework.DomainDriven.NHibernate
{
    internal class NHibDalFactory<TPersistentDomainObjectBase, TIdent> : IDALFactory<TPersistentDomainObjectBase, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    {
        private readonly NHibSession _session;


        public NHibDalFactory(NHibSession session)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));

            this._session = session;

            if (!this._session.SessionFactory.RegisteredTypes.Contains(typeof(TPersistentDomainObjectBase)))
            {
                throw new Exception($"Mapping Settings for type {typeof(TPersistentDomainObjectBase).FullName} not found");
            }
        }


        public IDAL<TDomainObject, TIdent> CreateDAL<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            return new NHibDal<TDomainObject, TIdent>(this._session);
        }
    }
}