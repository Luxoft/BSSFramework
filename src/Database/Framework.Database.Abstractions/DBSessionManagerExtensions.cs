using Anch.Core;

namespace Framework.Database;

public static class DbSessionManagerExtensions
{
    extension(IDBSessionManager dbSessionManager)
    {
        public Task EvaluateAsync(Func<Task> action, CancellationToken cancellationToken = default) =>
            dbSessionManager.EvaluateAsync(action.ToDefaultTask(), cancellationToken);

        public async Task<T> EvaluateAsync<T>(Func<Task<T>> getResult, CancellationToken cancellationToken = default)
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
    }
}
