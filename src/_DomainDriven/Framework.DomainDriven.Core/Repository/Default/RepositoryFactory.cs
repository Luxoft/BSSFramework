using Framework.DomainDriven.Repository.NotImplementedDomainSecurityService;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.Repository;

public class RepositoryFactory<TDomainObject> : TemplateRepositoryFactory<
                                                IRepository<TDomainObject>,
                                                Repository<TDomainObject>,
                                                TDomainObject>,

                                                IRepositoryFactory<TDomainObject>

    where TDomainObject : class
{
    public RepositoryFactory(
        IServiceProvider serviceProvider,
        INotImplementedDomainSecurityService<TDomainObject> notImplementedDomainSecurityService,
        IDomainSecurityService<TDomainObject> domainSecurityService = null)
        : base(serviceProvider, notImplementedDomainSecurityService, domainSecurityService)
    {
    }
}
