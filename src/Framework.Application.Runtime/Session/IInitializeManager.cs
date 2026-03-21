namespace Framework.Application.Session;

public interface IInitializeManager
{
    bool IsInitialize { get; }

    Task InitializeOperationAsync(Func<Task> operation);
}
