using Framework.DomainDriven.ScopedEvaluate;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven;

public class SessionEvaluatorMiddleware : IScopedEvaluatorMiddleware
{
    private readonly IServiceProvider scopedServiceProvider;

    private readonly DBSessionMode sessionMode;

    public SessionEvaluatorMiddleware(IServiceProvider scopedServiceProvider, DBSessionMode sessionMode)
    {
        this.scopedServiceProvider = scopedServiceProvider;
        this.sessionMode = sessionMode;
    }

    public async Task<TResult> EvaluateAsync<TResult>(Func<Task<TResult>> getResult)
    {
        await using var session = this.scopedServiceProvider.GetRequiredService<IDBSession>();

        if (this.sessionMode == DBSessionMode.Read)
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
