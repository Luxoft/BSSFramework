using CommonFramework;
using CommonFramework.DependencyInjection;

using Framework.Application.Events;
using Framework.Database.DALListener;
using Framework.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Application.DependencyInjection;

public class DALListenerSetup : IDALListenerSetup, IServiceInitializer
{
    private readonly List<Action<IServiceCollection>> initActions = [];

    public IDALListenerSetup Add<TListener>()
        where TListener : class, IDALListener
    {
        this.initActions.Add(this.AddListener<TListener>);

        return this;
    }

    public void Initialize(IServiceCollection services)
    {
        foreach (var setupObjectInitAction in this.initActions)
        {
            setupObjectInitAction(services);
        }
    }

    private void AddListener<TListener>(IServiceCollection services)
        where TListener : class, IDALListener
    {
        services.AddScoped<TListener>();

        var result = new[] { typeof(IAfterTransactionCompletedDALListener), typeof(IBeforeTransactionCompletedDALListener), typeof(IFlushedDALListener) }
                     .Select(interfaceType => this.TryAddCastService<TListener>(services, interfaceType))
                     .ToArray();

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
