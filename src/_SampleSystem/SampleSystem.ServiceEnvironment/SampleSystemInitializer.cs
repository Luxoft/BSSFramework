using Framework.Authorization.Domain;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.DomainDriven;
using Framework.DomainDriven.Lock;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;

using SecuritySystem.GeneralPermission.Initialize;

namespace SampleSystem.ServiceEnvironment;

public class SampleSystemInitializer(
    IServiceEvaluator<ISampleSystemBLLContext> contextEvaluator,
    IInitializeManager initializeManager)
{
    public async Task InitializeAsync(CancellationToken cancellationToken) =>
        await initializeManager.InitializeOperationAsync(async () => await this.InternalInitialize(cancellationToken));

    private async Task InternalInitialize(CancellationToken cancellationToken)
    {
        await contextEvaluator.EvaluateAsync(
            DBSessionMode.Write,
            async context => await context.ServiceProvider.GetRequiredService<INamedLockInitializer>().Initialize(cancellationToken));

        await this.InitSecurityAsync<ISecurityContextInitializer<SecurityContextType>>(cancellationToken);
        await this.InitSecurityAsync<ISecurityRoleInitializer<BusinessRole>>(cancellationToken);

        await contextEvaluator.EvaluateAsync(
            DBSessionMode.Write,
            async context => await context.ServiceProvider.GetRequiredService<ITargetSystemInitializer>().Initialize(cancellationToken));

        contextEvaluator.Evaluate(
            DBSessionMode.Write,
            context => context.Configuration.Logics.SystemConstant.Initialize(typeof(SampleSystemSystemConstant)));

        await contextEvaluator.EvaluateAsync(
            DBSessionMode.Write,
            async context => await context.ServiceProvider.GetRequiredService<ISubscriptionInitializer>().Initialize(cancellationToken));
    }

    private async Task InitSecurityAsync<TSecurityInitializer>(CancellationToken cancellationToken)
        where TSecurityInitializer : ISecurityInitializer =>
        await contextEvaluator.EvaluateAsync(
            DBSessionMode.Write,
            async context =>
                await context.ServiceProvider
                             .GetRequiredService<TSecurityInitializer>()
                             .Init(cancellationToken));
}
