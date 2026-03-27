using Framework.DomainDriven.ServiceModel.Service;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.WebApiNetCore;

public class ApiControllerBaseSingleCallEvaluator<TBLLContext, TMappingService>(IServiceProvider serviceProvider)
    : IApiControllerBaseEvaluator<TBLLContext, TMappingService>
{
    private bool evaluateInvoked;

    public TResult Evaluate<TResult>(DBSessionMode sessionMode, Func<EvaluatedData<TBLLContext, TMappingService>, TResult> getResult)
    {
        if (this.evaluateInvoked)
        {
            throw new Exception("Allowed single evaluate. For multiply session DON'T use this method. Use IContextEvaluator<,>");
        }

        this.evaluateInvoked = true;

        if (sessionMode == DBSessionMode.Read)
        {
            serviceProvider.GetRequiredService<IDBSession>().AsReadOnly();
        }

        return getResult(serviceProvider.GetRequiredService<EvaluatedData<TBLLContext, TMappingService>>());
    }
}


