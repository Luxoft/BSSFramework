using Framework.Database;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Application.Middleware;

public class SessionEvaluatorMiddleware(IServiceProvider scopedServiceProvider, DBSessionMode sessionMode) : IScopedEvaluatorMiddleware
{
    public async Task<TResult> EvaluateAsync<TResult>(Func<Task<TResult>> getResult, CancellationToken ct)
    {
        await using var session = scopedServiceProvider.GetRequiredService<IDBSession>();

        if (sessionMode == DBSessionMode.Read)
        {
            session.AsReadOnly();
        }

        try
        {
            return await getResult();
        }
        catch
        {
            session.AsFault();

            throw;
        }
        finally
        {
            await session.CloseAsync(ct);
        }
    }
}
