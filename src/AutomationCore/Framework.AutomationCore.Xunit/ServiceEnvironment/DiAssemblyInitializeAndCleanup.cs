using Bss.Testing.Xunit.Interfaces;

using Framework.AutomationCore.Settings;
using Framework.AutomationCore.Utils.DatabaseUtils.Interfaces;

using Microsoft.Extensions.Options;

namespace Framework.AutomationCore.Xunit.ServiceEnvironment;

public class DiAssemblyInitializeAndCleanup(
    IOptions<AutomationFrameworkSettings> settings,
    ITestDatabaseGenerator databaseGenerator)
    : AssemblyInitializeAndCleanupBase, IAssemblyInitializeAndCleanup
{
    private readonly AutomationFrameworkSettings settings = settings.Value;

    public async Task EnvironmentInitializeAsync() => await this.InitializeAsync(this.settings, databaseGenerator);

    public async Task EnvironmentCleanupAsync() => await this.CleanupAsync(this.settings, databaseGenerator);
}
