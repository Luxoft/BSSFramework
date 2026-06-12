using Anch.Core;

namespace Framework.Database;

public static class DbSessionManagerExtensions
{
    extension(IDBSessionManager dbSessionManager)
    {
        public Task EvaluateAsync(Func<Task> action, CancellationToken ct) =>
            dbSessionManager.EvaluateAsync(action.ToDefaultTask(), ct);

        public async Task<T> EvaluateAsync<T>(Func<Task<T>> getResult, CancellationToken ct)
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
                await dbSessionManager.TryCloseDbSessionAsync(ct);
            }
        }
    }
}

