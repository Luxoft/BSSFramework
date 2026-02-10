using Framework.DomainDriven;
using Framework.Persistent;

namespace Framework.Authorization.Environment;

public class MasterDetailDalGenericInterceptor<TDomainObject, TMaster> : IDalGenericInterceptor<TDomainObject>
    where TMaster : class, IMaster<TDomainObject>
    where TDomainObject : class, IDetail<TMaster>
{
    public async Task SaveAsync(TDomainObject data, CancellationToken cancellationToken) => data.Master?.RemoveDetail(data);

    public async Task RemoveAsync(TDomainObject data, CancellationToken cancellationToken) => data.Master?.RemoveDetail<TMaster, TDomainObject>(data);
}
