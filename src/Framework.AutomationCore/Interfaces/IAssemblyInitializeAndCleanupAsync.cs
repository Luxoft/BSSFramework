namespace Automation.Interfaces;

public interface IAssemblyInitializeAndCleanupAsync
{
    public Task EnvironmentInitializeAsync();

    public Task EnvironmentCleanupAsync();
}
