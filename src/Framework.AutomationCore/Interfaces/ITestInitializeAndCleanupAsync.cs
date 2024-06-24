namespace Automation.Interfaces;

public interface ITestInitializeAndCleanupAsync
{
    Task InitializeAsync();

    Task CleanupAsync();
}
