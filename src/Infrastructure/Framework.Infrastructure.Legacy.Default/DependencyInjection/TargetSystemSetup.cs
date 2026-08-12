using Anch.Core;
using Anch.DependencyInjection;

using Framework.BLL.Domain.TargetSystem;
using Framework.Configuration.BLL.TargetSystemService;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Infrastructure.DependencyInjection;

public class TargetSystemSetup : ITargetSystemSetup, IServiceInitializer
{
    private readonly List<Action<IServiceCollection>> registerActions = [];

    public bool RegisterBase { get; set; } = true;

    public bool RegisterAuthorization { get; set; } = true;

    public bool RegisterConfiguration { get; set; } = true;

    public ITargetSystemSetup AddTargetSystem(PersistentTargetSystemInfo targetSystemInfo)
    {
        this.registerActions.Add(sc =>
        {
            sc.AddSingleton<TargetSystemInfo>(targetSystemInfo);
            sc.AddSingleton(targetSystemInfo);
        });

        return this;
    }

    public void Initialize(IServiceCollection services)
    {
        services.AddScoped<ITargetSystemInitializer, TargetSystemInitializer>();

        if (this.RegisterBase)
        {
            services.AddSingleton(TargetSystemInfo.Base);
        }

        if (this.RegisterAuthorization)
        {
            this.AddTargetSystem(PersistentTargetSystemInfoHelper.Authorization);
        }

        if (this.RegisterConfiguration)
        {
            this.AddTargetSystem(PersistentTargetSystemInfoHelper.Configuration);
        }

        this.registerActions.Foreach(action => action(services));
    }
}
