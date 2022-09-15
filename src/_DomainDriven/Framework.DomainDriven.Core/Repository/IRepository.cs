using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.DomainDriven.Repository;

public interface IRepository<TDomainObject, in TIdent>
{
    Task SaveAsync(TDomainObject domainObject, CancellationToken cancellationToken);

    Task InsertAsync(TDomainObject domainObject, TIdent id, CancellationToken cancellationToken);

    Task RemoveAsync(TDomainObject domainObject, CancellationToken cancellationToken);

    IQueryable<TDomainObject> GetQueryable();
}
