using Automation.Settings;
using Automation.Utils.DatabaseUtils;

using Bss.Testing.Xunit.Interfaces;

using Microsoft.Extensions.Options;

namespace Automation.Xunit.ServiceEnvironment;

public class DiAssemblyInitializeAndCleanup(
    IOptions<AutomationFrameworkSettings> settings,
    ITestDatabaseGenerator databaseGenerator)
    : AssemblyInitializeAndCleanupBase, IAssemblyInitializeAndCleanup
{
    private readonly AutomationFrameworkSettings settings = settings.Value;

    public async Task EnvironmentInitializeAsync() => await this.InitializeAsync(this.settings, databaseGenerator);

    public async Task EnvironmentCleanupAsync() => await this.CleanupAsync(this.settings, databaseGenerator);
}
