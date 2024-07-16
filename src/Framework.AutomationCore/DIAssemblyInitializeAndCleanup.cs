using Automation.Interfaces;
using Automation.Settings;
using Automation.Utils.DatabaseUtils;

using Microsoft.Extensions.Options;

namespace Automation;

public class DiAssemblyInitializeAndCleanup : AssemblyInitializeAndCleanupBase, IAssemblyInitializeAndCleanup
{
    private readonly AutomationFrameworkSettings settings;
    private readonly ITestDatabaseGenerator databaseGenerator;

    public DiAssemblyInitializeAndCleanup(
        IOptions<AutomationFrameworkSettings> settings,
        ITestDatabaseGenerator databaseGenerator)
    {
        this.settings = settings.Value;
        this.databaseGenerator = databaseGenerator;
    }

    public async Task EnvironmentInitializeAsync() => await this.InitializeAsync(this.settings, this.databaseGenerator);

    public async Task EnvironmentCleanupAsync() => await this.CleanupAsync(this.settings, this.databaseGenerator);
}
