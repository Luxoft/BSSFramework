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
        IDomainSecurityService<TDomainObject> domainSecurityService)
        : base(serviceProvider, domainSecurityService)
    {
    }
}
