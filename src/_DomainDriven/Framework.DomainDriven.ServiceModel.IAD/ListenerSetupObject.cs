using Framework.Core;
using Framework.DependencyInjection;
using Framework.Events;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class ListenerSetupObject : IListenerSetupObject
{
    private List<Action<IServiceCollection>> initActions = new();

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
            typeof(IFlushedDALListener),
            typeof(IEventOperationReceiver)
        }.ToArray(interfaceType => this.TryCastListener<TListener>(services, interfaceType, registerSelf));

        if (!result.Any(v => v))
        {
            throw new Exception($"Invalid listener: {typeof(TListener)}");
        }
    }

    private bool TryCastListener<TCurrentListener>(IServiceCollection services, Type targetListenerList, bool registerSelf)
        where TCurrentListener : class, IDALListener
    {
        if (registerSelf)
        {
            if (targetListenerList.IsAssignableFrom(typeof(TCurrentListener)))
            {
                services.AddScoped(sp => (IBeforeTransactionCompletedDALListener)sp.GetRequiredService(typeof(TCurrentListener)));

                return true;
            }
        }
        else
        {
            if (targetListenerList.IsAssignableFrom(typeof(TCurrentListener)))
            {
                services.AddScoped(targetListenerList, typeof(TCurrentListener));

                return true;
            }
        }

        return false;
    }
}
