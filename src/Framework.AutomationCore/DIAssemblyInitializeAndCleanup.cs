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

    public void EnvironmentInitialize() => this.Initialize(this.settings, this.databaseGenerator);

    public void EnvironmentCleanup() => this.Cleanup(this.settings, this.databaseGenerator);
}
