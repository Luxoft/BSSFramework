using Framework.Core;
using Framework.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class ListenerSetupObject : IListenerSetupObject
{
    private List<Action<IServiceCollection>> initActions = new();

    public IReadOnlyList<Action<IServiceCollection>> InitActions => this.initActions;

    public IListenerSetupObject AddListener<TListener>(bool registerSelf = false)
        where TListener : class, IDALListener
    {
        this.initActions.Add(services => this.AddListener<TListener>(services, registerSelf));
    }

    private void AddListener<TListener>(IServiceCollection services, bool registerSelf)
        where TListener : class, IDALListener
    {
        if (registerSelf)
        {
            services.AddScoped<TListener>();
        }

        var manualEvent = typeof(TListener).GetInterfaceImplementationArgument(typeof(IManualEventDALListener<>))

        var result = new[]
                     {
                         typeof(IAfterTransactionCompletedDALListener),
                         typeof(IBeforeTransactionCompletedDALListener),
                         typeof(IFlushedDALListener),
                     }
            this.TryCastListener<TListener>(services, registerSelf);
    }

    private bool TryCastListener<TCurrentListener>(IServiceCollection services, Type targetListenerList, bool registerSelf)
        where TCurrentListener : class, IDALListener
    {
        if (registerSelf)
        {
            if (typeof(TTargetListener).IsAssignableFrom(typeof(TCurrentListener)))
            {
                services.AddScoped(sp => (IBeforeTransactionCompletedDALListener)sp.GetRequiredService(typeof(TCurrentListener)));
            }
        }
        else
        {
            if (typeof(TTargetListener).IsAssignableFrom(typeof(TCurrentListener)))
            {
                services.AddScoped(typeof(TTargetListener), typeof(TCurrentListener));
            }
        }
    }
}
