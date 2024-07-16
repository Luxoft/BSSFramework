namespace Automation.Interfaces;

public interface ITestInitializeAndCleanup
{
    Task InitializeAsync();

    Task CleanupAsync();
}
