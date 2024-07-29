namespace Automation.Interfaces;

public interface IAssemblyInitializeAndCleanup
{
    Task EnvironmentInitializeAsync();

    Task EnvironmentCleanupAsync();
}
