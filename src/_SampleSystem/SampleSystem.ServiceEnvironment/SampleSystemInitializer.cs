using Anch.Core;
using Anch.SecuritySystem.GeneralPermission.Initialize;

using Framework.Application.Lock;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.TargetSystemService;
using Framework.Database;

using Microsoft.Extensions.DependencyInjection;

namespace SampleSystem.ServiceEnvironment;

public class SampleSystemInitializer(
    IDBSessionEvaluator sessionEvaluator,
    IInitializeManager initializeManager)
{
    public async Task InitializeAsync(CancellationToken ct) =>
        await initializeManager.InitializeOperationAsync(async () => await this.InternalInitialize(ct));

    private async Task InternalInitialize(CancellationToken ct)
    {
        await this.Initialize<INamedLockInitializer>(ct);

        await this.Initialize<ISecurityContextInitializer>(ct);
        await this.Initialize<ISecurityRoleInitializer>(ct);

        await this.Initialize<ITargetSystemInitializer>(ct);
        await this.Initialize<ISystemConstantInitializer>(ct);
    }

    private async Task Initialize<TInitializer>(CancellationToken ct)
        where TInitializer : IInitializer =>
        await sessionEvaluator.EvaluateAsync(
            DBSessionMode.Write,
            serviceProvider => serviceProvider.GetRequiredService<TInitializer>().Initialize(ct),
            ct);
}
