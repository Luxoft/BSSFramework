using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Framework.Core;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.Repository;

public class Repository<TDomainObject, TIdent> : IRepository<TDomainObject, TIdent>
        where TDomainObject : class
{
    private readonly ISecurityProvider<TDomainObject> securityProvider;

    private readonly IAsyncDal<TDomainObject, TIdent> dal;

    public Repository(ISecurityProvider<TDomainObject> securityProvider, IAsyncDal<TDomainObject, TIdent> dal)
    {
        this.securityProvider = securityProvider;
        this.dal = dal;
    }

    public async Task SaveAsync(TDomainObject domainObject, CancellationToken cancellationToken)
    {
        this.securityProvider.CheckAccess(domainObject);

        await this.dal.SaveAsync(domainObject, cancellationToken);
    }

    public async Task InsertAsync(TDomainObject domainObject, TIdent id, CancellationToken cancellationToken)
    {
        this.securityProvider.CheckAccess(domainObject);

        await this.dal.InsertAsync(domainObject, id, cancellationToken);
    }

    public async Task RemoveAsync(TDomainObject domainObject, CancellationToken cancellationToken)
    {
        this.securityProvider.CheckAccess(domainObject);

        await this.dal.RemoveAsync(domainObject, cancellationToken);
    }

    public IQueryable<TDomainObject> GetQueryable()
    {
        return this.dal.GetQueryable().Pipe(this.securityProvider.InjectFilter);
    }
}
