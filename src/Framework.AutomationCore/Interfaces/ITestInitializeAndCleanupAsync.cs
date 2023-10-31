namespace Automation.Interfaces;

public interface ITestInitializeAndCleanupAsync
{
    public Task InitializeAsync();

    public Task CleanupAsync();
}
