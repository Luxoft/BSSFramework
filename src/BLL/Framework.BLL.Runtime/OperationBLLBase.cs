using Anch.Core;

using Framework.Application.Events;
using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.BLL;

public abstract class OperationBLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject>(TBLLContext context)
    : BLLContextContainer<TBLLContext>(context), IOperationBLLBase<TDomainObject>
    where TPersistentDomainObjectBase : class
    where TDomainObject : class, TPersistentDomainObjectBase
    where TBLLContext : class, IBLLOperationEventContext, IServiceProviderContainer
{
    protected IDefaultCancellationTokenSource? DefaultCancellationTokenSource => field ??= context.ServiceProvider.GetService<IDefaultCancellationTokenSource>();

    public virtual void Save(TDomainObject domainObject)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        this.DefaultCancellationTokenSource.RunSync(ct => this.Context.OperationSender.Send(domainObject, EventOperation.Save, ct));
    }

    public virtual void Remove(TDomainObject domainObject)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        this.DefaultCancellationTokenSource.RunSync(ct => this.Context.OperationSender.Send(domainObject, EventOperation.Remove, ct));
    }
}
