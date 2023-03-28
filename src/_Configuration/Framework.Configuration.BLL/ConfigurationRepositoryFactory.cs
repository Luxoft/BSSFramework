using Framework.Configuration.BLL.Core.Context;
using Framework.Configuration.Domain;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

namespace Framework.Configuration.BLL;

public class ConfigurationRepositoryFactory<TDomainObject> : RepositoryFactory<TDomainObject, Guid, ConfigurationSecurityOperationCode>,
                                                             IConfigurationRepositoryFactory<TDomainObject>
    where TDomainObject : PersistentDomainObjectBase
{
    public ConfigurationRepositoryFactory(
        IServiceProvider serviceProvider,
        INotImplementedDomainSecurityServiceContainer notImplementedDomainSecurityServiceContainer,
        IDomainSecurityService<TDomainObject, ConfigurationSecurityOperationCode> domainSecurityService)
        : base(serviceProvider, notImplementedDomainSecurityServiceContainer, domainSecurityService)
    {
    }
}
