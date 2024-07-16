namespace Framework.DomainDriven.ServiceModel.IAD;

public interface IInitializeManager
{
    bool IsInitialize { get; }

    Task InitializeOperationAsync(Func<Task> operation);
}
