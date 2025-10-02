namespace Framework.DomainDriven.ApplicationCore.DALListeners;

public class DenormalizeHierarchicalDALListener(IServiceProvider serviceProvider) : IBeforeTransactionCompletedDALListener
{
    public void Process(DALChangesEventArgs eventArgs)
    {
        throw new NotImplementedException("IvAt");
    }
}
