namespace Automation.Interfaces;

public interface IAssemblyInitializeAndCleanup
{
    public void EnvironmentInitialize();

    public void EnvironmentCleanup();
}
