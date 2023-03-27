using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.WebApiNetCore;

public class ApiControllerBaseSingleCallEvaluator<TEvaluatedData> : IApiControllerBaseEvaluator<TEvaluatedData>
{
    private readonly IServiceProvider serviceProvider;

    private bool evaluateInvoked;

    public ApiControllerBaseSingleCallEvaluator(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public TResult Evaluate<TResult>(DBSessionMode sessionMode, Func<TEvaluatedData, TResult> getResult)
    {
        if (this.evaluateInvoked)
        {
            throw new Exception("Allowed single evaluate. For multiply session DON'T use this method. Use IContextEvaluator<,>");
        }

        this.evaluateInvoked = true;

        if (sessionMode == DBSessionMode.Read)
        {
            this.serviceProvider.GetRequiredService<IDBSession>().AsReadOnly();
        }

        return getResult(this.serviceProvider.GetRequiredService<TEvaluatedData>());
    }
}


