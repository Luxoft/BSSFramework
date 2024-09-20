using Framework.DomainDriven.ScopedEvaluate;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven;

public class SessionEvaluatorMiddleware(IServiceProvider scopedServiceProvider, DBSessionMode sessionMode) : IScopedEvaluatorMiddleware
{
    public async Task<TResult> EvaluateAsync<TResult>(Func<Task<TResult>> getResult)
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
    }
}
