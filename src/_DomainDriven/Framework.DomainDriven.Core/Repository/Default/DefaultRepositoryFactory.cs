using Framework.SecuritySystem;

using JetBrains.Annotations;

namespace Framework.DomainDriven.Repository;

public class DefaultRepositoryFactory<TDomainObject, TSecurityOperationCode> : TemplateRepositoryFactory<
                                                                               IRepository<TDomainObject>,
                                                                               Repository<TDomainObject>,
                                                                               TDomainObject,
                                                                               TSecurityOperationCode>,

                                                                               IDefaultRepositoryFactory<TDomainObject, TSecurityOperationCode>

    where TDomainObject : class
    where TSecurityOperationCode : struct, Enum
{
    public DefaultRepositoryFactory(
        IServiceProvider serviceProvider,
        INotImplementedDomainSecurityServiceContainer notImplementedDomainSecurityServiceContainer,
        [CanBeNull] IDomainSecurityService<TDomainObject, TSecurityOperationCode> domainSecurityService = null)
        : base(serviceProvider, notImplementedDomainSecurityServiceContainer, domainSecurityService)
    {
    }
}

public class DefaultRepositoryFactory<TDomainObject> : TemplateRepositoryFactory<
                                                       IRepository<TDomainObject>,
                                                       Repository<TDomainObject>,
                                                       TDomainObject>,
                                                       IDefaultRepositoryFactory<TDomainObject>

    where TDomainObject : class
{
    public DefaultRepositoryFactory(
        IServiceProvider serviceProvider,
        INotImplementedDomainSecurityServiceContainer notImplementedDomainSecurityServiceContainer,
        [CanBeNull] IDomainSecurityService<TDomainObject> domainSecurityService = null)
        : base(serviceProvider, notImplementedDomainSecurityServiceContainer, domainSecurityService)
    {
    }
}
