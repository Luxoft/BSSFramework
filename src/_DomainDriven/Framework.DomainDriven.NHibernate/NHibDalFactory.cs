using System;

using Framework.Persistent;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.NHibernate
{
    public class NHibDalFactory<TPersistentDomainObjectBase, TIdent> : IDALFactory<TPersistentDomainObjectBase, TIdent>
        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    {
        private readonly IServiceProvider serviceProvider;

        private readonly NHibSessionBase session;


        public NHibDalFactory(IServiceProvider serviceProvider, NHibSessionBase session)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.session = session ?? throw new ArgumentNullException(nameof(session));

            if (!this.session.Environment.RegisteredTypes.Contains(typeof(TPersistentDomainObjectBase)))
            {
                throw new Exception($"Mapping Settings for type {typeof(TPersistentDomainObjectBase).FullName} not found");
            }
        }


        public IDAL<TDomainObject, TIdent> CreateDAL<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            return ActivatorUtilities.CreateInstance<NHibDal<TDomainObject, TIdent>>(this.serviceProvider);
        }
    }
}
