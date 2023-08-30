using Framework.SecuritySystem;



namespace Framework.DomainDriven.Repository;

public class RepositoryFactory<TDomainObject, TSecurityOperationCode> : TemplateRepositoryFactory<
                                                                               IRepository<TDomainObject>,
                                                                               Repository<TDomainObject>,
                                                                               TDomainObject,
                                                                               TSecurityOperationCode>,

                                                                               IRepositoryFactory<TDomainObject, TSecurityOperationCode>

    where TDomainObject : class
    where TSecurityOperationCode : struct, Enum
{
    public RepositoryFactory(
        IServiceProvider serviceProvider,
        INotImplementedDomainSecurityServiceContainer notImplementedDomainSecurityServiceContainer,
        IDomainSecurityService<TDomainObject, TSecurityOperationCode> domainSecurityService = null)
        : base(serviceProvider, notImplementedDomainSecurityServiceContainer, domainSecurityService)
    {
    }
}

public class RepositoryFactory<TDomainObject> : TemplateRepositoryFactory<
                                                       IRepository<TDomainObject>,
                                                       Repository<TDomainObject>,
                                                       TDomainObject>,
                                                       IRepositoryFactory<TDomainObject>

    where TDomainObject : class
{
    public RepositoryFactory(
        IServiceProvider serviceProvider,
        INotImplementedDomainSecurityServiceContainer notImplementedDomainSecurityServiceContainer,
        IDomainSecurityService<TDomainObject> domainSecurityService = null)
        : base(serviceProvider, notImplementedDomainSecurityServiceContainer, domainSecurityService)
    {
    }
}
