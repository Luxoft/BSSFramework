using Automation.Interfaces;
using Automation.Utils;
using Automation.Utils.DatabaseUtils;

namespace Automation;

public class DiAssemblyInitializeAndCleanup : AssemblyInitializeAndCleanupBase, IAssemblyInitializeAndCleanup
{
    private readonly ConfigUtil configUtil;
    private readonly ITestDatabaseGenerator databaseGenerator;

    public DiAssemblyInitializeAndCleanup(
        ConfigUtil configUtil,
        ITestDatabaseGenerator databaseGenerator)
    {
        this.configUtil = configUtil;
        this.databaseGenerator = databaseGenerator;
    }

    public void EnvironmentInitialize() => this.Initialize(this.configUtil, this.databaseGenerator);

    public void EnvironmentCleanup() => this.Cleanup(this.configUtil, this.databaseGenerator);
}
