namespace Framework.DomainDriven.ServiceModel.IAD;

public interface IInitializeManager
{
    bool IsInitialize { get; }

    void InitializeOperation(Action operation) => this.InitializeOperationAsync(() => Task.Run(operation)).GetAwaiter().GetResult();

    Task InitializeOperationAsync(Func<Task> operation);
}
