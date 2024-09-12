using Framework.DomainDriven;

namespace SampleSystem.ServiceEnvironment;

public class ExampleFaultDALListener(ExampleFaultDALListenerSettings settings) : IBeforeTransactionCompletedDALListener
{
    public void Process(DALChangesEventArgs eventArgs)
    {
        if (settings.Raise)
        {
            throw new Exception(nameof(ExampleFaultDALListener));
        }
    }
}

public class ExampleFaultDALListenerSettings
{
    public bool Raise { get; set; }
}
