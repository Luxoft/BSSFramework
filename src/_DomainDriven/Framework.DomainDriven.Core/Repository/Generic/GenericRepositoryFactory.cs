using Framework.SecuritySystem;

using JetBrains.Annotations;

namespace Framework.DomainDriven.Repository;

public class GenericRepositoryFactory<TDomainObject, TIdent, TSecurityOperationCode> : TemplateRepositoryFactory<
    IGenericRepository<TDomainObject, TIdent>,
    GenericRepository<TDomainObject, TIdent>,
    TDomainObject,
    TSecurityOperationCode>,

    IGenericRepositoryFactory<TDomainObject, TIdent, TSecurityOperationCode>

    where TDomainObject : class
    where TSecurityOperationCode : struct, Enum
{
    public GenericRepositoryFactory(
        IServiceProvider serviceProvider,
        INotImplementedDomainSecurityServiceContainer notImplementedDomainSecurityServiceContainer,
        [CanBeNull] IDomainSecurityService<TDomainObject, TSecurityOperationCode> domainSecurityService = null)
        : base(serviceProvider, notImplementedDomainSecurityServiceContainer, domainSecurityService)
    {
    }
}
