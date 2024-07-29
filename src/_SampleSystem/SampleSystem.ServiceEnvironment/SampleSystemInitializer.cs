using Framework.Authorization.SecuritySystem.Initialize;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.NamedLocks;
using Framework.DomainDriven;
using Framework.DomainDriven.ServiceModel.IAD;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;

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

        await this.InitSecurityAsync<IAuthorizationSecurityContextInitializer>(cancellationToken);
        await this.InitSecurityAsync<IAuthorizationBusinessRoleInitializer>(cancellationToken);

        contextEvaluator.Evaluate(
            DBSessionMode.Write,
            context => context.ServiceProvider.GetRequiredService<ITargetSystemInitializer>().Init());

        contextEvaluator.Evaluate(
            DBSessionMode.Write,
            context => { context.Configuration.Logics.SystemConstant.Initialize(typeof(SampleSystemSystemConstant)); });

        contextEvaluator.Evaluate(
            DBSessionMode.Write,
            context => context.ServiceProvider.GetRequiredService<ISubscriptionInitializer>().Init());
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
