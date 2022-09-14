using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Framework.DomainDriven;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace SampleSystem.BLL
{
    public interface IRepository<TDomainObject>
    {
        Task SaveAsync(TDomainObject domainObject, CancellationToken cancellationToken);

        Task RemoveAsync(TDomainObject domainObject, CancellationToken cancellationToken);

        IQueryable<TDomainObject> GetQueryable();
    }

    public interface Repository<TDomainObject>
    {
        public Repository(IServiceProvider serviceProvider, IDALFactory<>,>)
        {
            IDAL<>
        }

        Task SaveAsync(TDomainObject domainObject, CancellationToken cancellationToken);

        Task RemoveAsync(TDomainObject domainObject, CancellationToken cancellationToken);

        IQueryable<TDomainObject> GetQueryable();
    }

    public interface IRepositoryFactory<TDomainObject, TSecurityOperationCode>
            where TSecurityOperationCode : struct, Enum
    {
        IRepository<TDomainObject> Create(ISecurityProvider<TDomainObject> securityProvider);

        IRepository<TDomainObject> Create(TSecurityOperationCode securityOperationCode);

        IRepository<TDomainObject> Create(SecurityOperation<TSecurityOperationCode> securityOperation);


        IRepository<TDomainObject> Create(BLLSecurityMode securityMode);
    }

    public class RepositoryFactory<TDomainObject, TSecurityOperationCode> : IRepositoryFactory<TDomainObject, TSecurityOperationCode>
            where TSecurityOperationCode : struct, Enum
    {
        private readonly IServiceProvider serviceProvider;

        private readonly IDomainSecurityService<TDomainObject, TSecurityOperationCode> domainSecurityService;

        public RepositoryFactory(IServiceProvider serviceProvider, IDomainSecurityService<TDomainObject, TSecurityOperationCode> domainSecurityService)
        {
            this.serviceProvider = serviceProvider;
            this.domainSecurityService = domainSecurityService;
        }

        public IRepository<TDomainObject> Create(ISecurityProvider<TDomainObject> securityProvider)
        {
            return ActivatorUtilities.CreateInstance<Repository<TDomainObject>>(this.serviceProvider, securityProvider);
        }

        public IRepository<TDomainObject> Create(TSecurityOperationCode securityOperationCode) => throw new NotImplementedException();

        public IRepository<TDomainObject> Create(SecurityOperation<TSecurityOperationCode> securityOperation) => throw new NotImplementedException();

        public IRepository<TDomainObject> Create(BLLSecurityMode securityMode) => throw new NotImplementedException();
    }
}
