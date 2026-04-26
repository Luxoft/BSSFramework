using Anch.Core;

using Framework.Application.Lock;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.TargetSystemService;
using Framework.Database;

using Microsoft.Extensions.DependencyInjection;

using Anch.SecuritySystem.GeneralPermission.Initialize;

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
    }

    private async Task Initialize<TInitializer>(CancellationToken cancellationToken)
        where TInitializer : IInitializer =>
        await sessionEvaluator.EvaluateAsync(
            DBSessionMode.Write,
            serviceProvider => serviceProvider.GetRequiredService<TInitializer>().Initialize(cancellationToken));
}
