using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;

/// <summary>
/// Процессор лямбда-выражения типа "CopyGeneration".
/// </summary>
/// <seealso cref="LambdaProcessor" />
public class GenerationLambdaProcessorCc<TBLLContext> : GenerationLambdaProcessorBase<TBLLContext>
        where TBLLContext : class
{
    /// <summary>Создаёт экземпляр класса <see cref="GenerationLambdaProcessorCc"/>.</summary>
    /// <param name="bllContext">Контекст бизнес-логики.</param>
    public GenerationLambdaProcessorCc(TBLLContext bllContext)
            : base(bllContext)
    {
    }

    /// <inheritdoc/>
    protected override string LambdaName => "CopyGeneration";

    /// <inheritdoc/>
    protected override SubscriptionLambda GetSubscriptionLambda(Subscription subscription)
    {
        return subscription.CopyGeneration;
    }
}
