using Framework.Core;
using Framework.Events;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class ListenerSetupObject : IListenerSetupObject
{
    private readonly List<Action<IServiceCollection>> initActions = new();

    public IReadOnlyList<Action<IServiceCollection>> InitActions => this.initActions;

    public IListenerSetupObject Add<TListener>(bool registerSelf = false)
        where TListener : class, IDALListener
    {
        this.initActions.Add(services => this.AddListener<TListener>(services, registerSelf));

        return this;
    }

    private void AddListener<TListener>(IServiceCollection services, bool registerSelf)
        where TListener : class, IDALListener
    {
        if (registerSelf)
        {
            services.AddScoped<TListener>();
        }

        var result = new[]
        {
            typeof(IAfterTransactionCompletedDALListener),
            typeof(IBeforeTransactionCompletedDALListener),
            typeof(IFlushedDALListener)
        }.ToArray(interfaceType => this.TryCastService<TListener>(services, interfaceType, registerSelf));

        this.TryCastService<TListener>(services, typeof(IEventOperationReceiver), registerSelf);

        if (!result.Any(v => v))
        {
            throw new Exception($"Invalid listener: {typeof(TListener)}");
        }
    }

    private bool TryCastService<TCurrentListener>(IServiceCollection services, Type targetServiceType, bool registerSelf)
        where TCurrentListener : class, IDALListener
    {
        if (registerSelf)
        {
            if (targetServiceType.IsAssignableFrom(typeof(TCurrentListener)))
            {
                services.AddScoped(sp => (IBeforeTransactionCompletedDALListener)sp.GetRequiredService(typeof(TCurrentListener)));

                return true;
            }
        }
        else
        {
            if (targetServiceType.IsAssignableFrom(typeof(TCurrentListener)))
            {
                services.AddScoped(targetServiceType, typeof(TCurrentListener));

                return true;
            }
        }

        return false;
    }
}
