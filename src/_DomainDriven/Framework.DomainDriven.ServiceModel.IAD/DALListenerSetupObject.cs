using Framework.Core;
using Framework.DependencyInjection;
using Framework.Events;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class DALListenerSetupObject : IDALListenerSetupObject
{
    private readonly List<Action<IServiceCollection>> initActions = new();

    public IReadOnlyList<Action<IServiceCollection>> InitActions => this.initActions;

    public IDALListenerSetupObject Add<TListener>()
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

    private bool TryAddCastService<TListener>(IServiceCollection services, Type targetServiceType)
        where TListener : class, IDALListener
    {
        if (targetServiceType.IsAssignableFrom(typeof(TListener)))
        {
            new Action<IServiceCollection>(this.AddService<TListener, TListener>)
                .CreateGenericMethod(targetServiceType, typeof(TListener))
                .Invoke(this, [services]);

            return true;
        }

        return false;
    }

    private void AddService<TService, TListener>(IServiceCollection services)
        where TListener : class, IDALListener, TService
        where TService : class =>
        services.AddScopedFromLazyInterfaceImplement<TService, TListener>(false);
}
