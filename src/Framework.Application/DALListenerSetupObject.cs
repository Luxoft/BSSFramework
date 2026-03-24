using CommonFramework;

using Framework.Application.DALListener;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Application.DALListeners;

public class DALListenerSetupObject : IDALListenerSetupObject
{
    private readonly List<Action<IServiceCollection>> initActions = [];

    public IReadOnlyList<Action<IServiceCollection>> InitActions => this.initActions;

    public IDALListenerSetupObject Add<TListener>()
        where TListener : class, IdalListener
    {
        this.initActions.Add(this.AddListener<TListener>);

        return this;
    }

    private void AddListener<TListener>(IServiceCollection services)
        where TListener : class, IdalListener
    {
        services.AddScoped<TListener>();

        var result = new[]
                     {
                         typeof(IAfterTransactionCompletedDalListener),
                         typeof(IBeforeTransactionCompletedDalListener),
                         typeof(IFlushedDalListener)
                     }.ToArray(interfaceType => this.TryAddCastService<TListener>(services, interfaceType));

        this.TryAddCastService<TListener>(services, typeof(IEventOperationReceiver));

        if (!result.Any(v => v))
        {
            throw new Exception($"Invalid listener: {typeof(TListener)}");
        }
    }

    private bool TryAddCastService<TListener>(IServiceCollection services, Type targetServiceType)
        where TListener : class, IdalListener
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
        where TListener : class, IdalListener, TService
        where TService : class =>
        services.AddScopedFromLazyInterfaceImplement<TService, TListener>(false);
}
