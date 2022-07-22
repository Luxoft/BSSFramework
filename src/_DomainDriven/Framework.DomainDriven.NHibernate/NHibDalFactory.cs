using System;

using Framework.DomainDriven.BLL;
using Framework.Persistent;

namespace Framework.DomainDriven.NHibernate
{
    internal class NHibDalFactory<TPersistentDomainObjectBase, TIdent> : IDALFactory<TPersistentDomainObjectBase, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    {
        private readonly NHibSessionBase session;


        public NHibDalFactory(NHibSessionBase session)
        {
            this.session = session ?? throw new ArgumentNullException(nameof(session));

            if (!this.session.Environment.RegisteredTypes.Contains(typeof(TPersistentDomainObjectBase)))
            {
                throw new Exception($"Mapping Settings for type {typeof(TPersistentDomainObjectBase).FullName} not found");
            }
        }


        public IDAL<TDomainObject, TIdent> CreateDAL<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            return new NHibDal<TDomainObject, TIdent>(this.session);
        }
    }
}
