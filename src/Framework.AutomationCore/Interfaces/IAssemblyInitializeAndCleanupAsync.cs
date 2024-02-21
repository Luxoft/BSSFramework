namespace Automation.Interfaces;

public interface IAssemblyInitializeAndCleanupAsync
{
    Task EnvironmentInitializeAsync();

    Task EnvironmentCleanupAsync();
}
