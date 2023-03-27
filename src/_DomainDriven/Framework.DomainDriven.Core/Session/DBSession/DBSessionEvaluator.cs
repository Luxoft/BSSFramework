using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven;

public class DBSessionEvaluator : IDBSessionEvaluator
{
    private readonly IServiceProvider rootServiceProvider;

    public DBSessionEvaluator([NotNull] IServiceProvider rootServiceProvider)
    {
        this.rootServiceProvider = rootServiceProvider ?? throw new ArgumentNullException(nameof(rootServiceProvider));
    }

    public async Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, Func<IServiceProvider, IDBSession, Task<TResult>> getResult)
    {
        await using var scope = this.rootServiceProvider.CreateAsyncScope();

        var scopeServiceProvider = scope.ServiceProvider;
        using var session = scopeServiceProvider.GetRequiredService<IDBSession>();

        if (sessionMode == DBSessionMode.Read)
        {
            session.AsReadOnly();
        }

        try
        {
            return await getResult(scopeServiceProvider, session);
        }
        catch
        {
            session.AsFault();

            throw;
        }
    }
}
