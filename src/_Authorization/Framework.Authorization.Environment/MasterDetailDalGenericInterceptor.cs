using Framework.Application;
using Framework.Relations;

namespace Framework.Authorization.Environment;

public class MasterDetailDalGenericInterceptor<TDomainObject, TMaster> : IDalGenericInterceptor<TDomainObject>
    where TMaster : class, IMaster<TDomainObject>
    where TDomainObject : class, IDetail<TMaster>
{
    public async Task SaveAsync(TDomainObject data, CancellationToken ct)
    {
        if (data.Master != null && !data.Master.Details.Contains(data))
        {
            data.Master.AddDetail(data);
        }
    }

    public async Task RemoveAsync(TDomainObject data, CancellationToken ct) => data.Master?.RemoveDetail<TMaster, TDomainObject>(data);
}

