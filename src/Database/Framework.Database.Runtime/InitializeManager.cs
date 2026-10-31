namespace Framework.Database;

public class InitializeManager : IInitializeManager
{
    /// <summary>
    /// Flag indicating that the system is being initialized (subscriptions to all events are disabled in this state)
    /// </summary>
    public bool IsInitialize { get; private set; }

    public async Task InitializeOperationAsync(Func<Task> operation)
    {
        if (operation is null) throw new ArgumentNullException(nameof(operation));

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
