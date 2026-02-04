using CommonFramework;

using Framework.Configuration.BLL;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.DomainDriven;
using Framework.DomainDriven.Lock;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem.GeneralPermission.Initialize;

namespace SampleSystem.ServiceEnvironment;

public class SampleSystemInitializer(
    IDBSessionEvaluator sessionEvaluator,
    IInitializeManager initializeManager)
{
    public async Task InitializeAsync(CancellationToken cancellationToken) =>
        await initializeManager.InitializeOperationAsync(async () => await this.InternalInitialize(cancellationToken));

    private async Task InternalInitialize(CancellationToken cancellationToken)
    {
        await this.Initialize<INamedLockInitializer>(cancellationToken);

        await this.Initialize<ISecurityContextInitializer>(cancellationToken);
        await this.Initialize<ISecurityRoleInitializer>(cancellationToken);

        await this.Initialize<ITargetSystemInitializer>(cancellationToken);
        await this.Initialize<ISystemConstantInitializer>(cancellationToken);

        await this.Initialize<ISubscriptionInitializer>(cancellationToken);
    }

    private async Task Initialize<TInitializer>(CancellationToken cancellationToken)
        where TInitializer : IInitializer =>
        await sessionEvaluator.EvaluateAsync(
            DBSessionMode.Write,
            serviceProvider => serviceProvider.GetRequiredService<TInitializer>().Initialize(cancellationToken));
}
