namespace Framework.DomainDriven;

public static class DBSessionManagerExtensions
{
    public static async Task<T> EvaluateAsync<T>(this IDBSessionManager dbSessionManager, CancellationToken cancellationToken, Func<Task<T>> getResult)
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

    public static async Task EvaluateAsync(this IDBSessionManager dbSessionManager, CancellationToken cancellationToken, Func<Task> action)
    {
        await dbSessionManager.EvaluateAsync<object>(
            cancellationToken,
            async () =>
            {
                await action();
                return default;
            });
    }
}
