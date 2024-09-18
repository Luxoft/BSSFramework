namespace Framework.DomainDriven;

public interface IInitializeManager
{
    bool IsInitialize { get; }

    Task InitializeOperationAsync(Func<Task> operation);
}
