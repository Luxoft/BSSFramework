using System;

using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.Repository;

public class RepositoryFactory<TDomainObject, TIdent, TSecurityOperationCode> : IRepositoryFactory<TDomainObject, TIdent,
        TSecurityOperationCode>
        where TDomainObject : class
        where TSecurityOperationCode : struct, Enum
{
    private readonly IServiceProvider serviceProvider;

    private readonly IDomainSecurityService<TDomainObject, TSecurityOperationCode> domainSecurityService;

    public RepositoryFactory(
            IServiceProvider serviceProvider,
            INotImplementedDomainSecurityServiceContainer notImplementedDomainSecurityServiceContainer,
            IDomainSecurityService<TDomainObject, TSecurityOperationCode>? domainSecurityService = null)
    {
        this.serviceProvider = serviceProvider;
        this.domainSecurityService = domainSecurityService ?? notImplementedDomainSecurityServiceContainer.GetNotImplementedDomainSecurityService<TDomainObject, TSecurityOperationCode>();
    }

    public IRepository<TDomainObject, TIdent> Create(ISecurityProvider<TDomainObject> securityProvider) =>
            ActivatorUtilities.CreateInstance<Repository<TDomainObject, TIdent>>(this.serviceProvider, securityProvider);

    public IRepository<TDomainObject, TIdent> Create(TSecurityOperationCode securityOperationCode) =>
            this.Create(this.domainSecurityService.GetSecurityProvider(securityOperationCode));

    public IRepository<TDomainObject, TIdent> Create(SecurityOperation<TSecurityOperationCode> securityOperation) =>
            this.Create(this.domainSecurityService.GetSecurityProvider(securityOperation));

    public IRepository<TDomainObject, TIdent> Create(BLLSecurityMode securityMode) =>
            this.Create(this.domainSecurityService.GetSecurityProvider(securityMode));

    public IRepository<TDomainObject, TIdent> Create() =>
            this.Create(this.domainSecurityService.GetSecurityProvider(BLLSecurityMode.Disabled));
}
