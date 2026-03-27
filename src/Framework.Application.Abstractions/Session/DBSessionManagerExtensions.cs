namespace Framework.Application.Session;

public static class DbSessionManagerExtensions
{
    public static async Task<T> EvaluateAsync<T>(this IDBSessionManager dbSessionManager, Func<Task<T>> getResult, CancellationToken cancellationToken = default)
    {
        try
        {
            return await getResult();
        }
        catch
        {
            dbSessionManager.TryFaultDbSession();
            throw;
        }
        finally
        {
            await dbSessionManager.TryCloseDbSessionAsync(cancellationToken);
        }
    }

    public static async Task EvaluateAsync(this IDBSessionManager dbSessionManager, Func<Task> action, CancellationToken cancellationToken = default)
    {
        await dbSessionManager.EvaluateAsync<object?>(
            async () =>
            {
                await action();
                return default;
            },
            cancellationToken);
    }
}
