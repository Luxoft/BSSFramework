using Framework.Core;
using Framework.DependencyInjection;
using Framework.Events;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class ListenerSetupObject : IListenerSetupObject
{
    private readonly List<Action<IServiceCollection>> initActions = new();

    public IReadOnlyList<Action<IServiceCollection>> InitActions => this.initActions;

    public IListenerSetupObject Add<TListener>()
        where TListener : class, IDALListener
    {
        this.initActions.Add(this.AddListener<TListener>);

        return this;
    }

    private void AddListener<TListener>(IServiceCollection services)
        where TListener : class, IDALListener
    {
        services.AddScoped<TListener>();

        var result = new[]
                     {
                         typeof(IAfterTransactionCompletedDALListener),
                         typeof(IBeforeTransactionCompletedDALListener),
                         typeof(IFlushedDALListener)
                     }.ToArray(interfaceType => this.TryAddCastService<TListener>(services, interfaceType));

        this.TryAddCastService<TListener>(services, typeof(IEventOperationReceiver));

        if (!result.Any(v => v))
        {
            throw new Exception($"Invalid listener: {typeof(TListener)}");
        }
    }

    private bool TryAddCastService<TCurrentListener>(IServiceCollection services, Type targetServiceType)
        where TCurrentListener : class, IDALListener
    {
        if (targetServiceType.IsAssignableFrom(typeof(TCurrentListener)))
        {
            new Action<IServiceCollection>(this.AddService<TCurrentListener, TCurrentListener>)
                .CreateGenericMethod(targetServiceType, typeof(TCurrentListener))
                .Invoke(this, [services]);

            return true;
        }

        return false;
    }

    private void AddService<TService, TCurrentListener>(IServiceCollection services)
        where TCurrentListener : class, IDALListener, TService
        where TService : class =>
        services.AddScopedFromLazyInterfaceImplement<TService, TCurrentListener>(false);
}
