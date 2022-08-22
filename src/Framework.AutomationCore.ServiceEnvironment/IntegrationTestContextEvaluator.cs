using System;
using System.Threading.Tasks;
using Automation.Utils;
using Automation.Utils.DatabaseUtils.Interfaces;
using Framework.DomainDriven;
using Microsoft.Extensions.DependencyInjection;

namespace Automation;

public abstract class IntegrationTestContextEvaluator<TBLLContext> : IContextEvaluator<TBLLContext>
{
    public IntegrationTestContextEvaluator(IServiceProvider rootServiceProvider)
    {
        this.RootServiceProvider = rootServiceProvider;
    }

    public virtual IServiceProvider RootServiceProvider { get; }

    public IDatabaseContext DatabaseContext => this.RootServiceProvider.GetRequiredService<IDatabaseContext>();

    protected ConfigUtil ConfigUtil => this.RootServiceProvider.GetRequiredService<ConfigUtil>();

    public Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, string customPrincipalName, Func<TBLLContext, IDBSession, Task<TResult>> getResult)
    {
        return this.RootServiceProvider.GetRequiredService<IContextEvaluator<TBLLContext>>().EvaluateAsync(sessionMode, customPrincipalName, getResult);
    }

    public TResult EvaluateWrite<TResult>(Func<TBLLContext, TResult> func)
    {
        return this.Evaluate(DBSessionMode.Write, func);
    }

    public void EvaluateWrite(Action<TBLLContext> action)
    {
        this.Evaluate(DBSessionMode.Write, action);
    }

    public void EvaluateRead(Action<TBLLContext> action)
    {
        this.Evaluate(DBSessionMode.Read, action);
    }

    public TResult EvaluateRead<TResult>(Func<TBLLContext, TResult> action)
    {
        return this.Evaluate(DBSessionMode.Read, action);
    }
}