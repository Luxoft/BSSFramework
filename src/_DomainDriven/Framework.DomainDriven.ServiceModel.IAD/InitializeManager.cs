namespace Framework.DomainDriven.ServiceModel.IAD;

public class InitializeManager : IInitializeManager
{
    /// <summary>
    /// Флаг, указывающий, что происходит инициализация системы (в этом состояния отключены подписки на все евенты)
    /// </summary>
    public bool IsInitialize { get; private set; }

    public async Task InitializeOperationAsync(Func<Task> operation)
    {
        if (operation == null) throw new ArgumentNullException(nameof(operation));

        if (this.IsInitialize)
        {
            throw new Exception("already initializing");
        }
        else
        {
            this.IsInitialize = true;

            try
            {
                await operation();
            }
            finally
            {
                this.IsInitialize = false;
            }
        }
    }
}
