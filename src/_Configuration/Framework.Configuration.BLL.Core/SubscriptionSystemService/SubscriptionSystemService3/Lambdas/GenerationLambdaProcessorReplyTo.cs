using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;

/// <summary>
/// Процессор лямбда-выражения типа "Generation".
/// </summary>
/// <seealso cref="LambdaProcessor" />
public class GenerationLambdaProcessorReplyTo<TBLLContext> : GenerationLambdaProcessorBase<TBLLContext>
        where TBLLContext : class
{
    /// <summary>Создаёт экземпляр класса <see cref="GenerationLambdaProcessorTo"/>.</summary>
    /// <param name="bllContext">Контекст бизнес-логики.</param>
    public GenerationLambdaProcessorReplyTo(TBLLContext bllContext)
            : base(bllContext)
    {
    }

    /// <inheritdoc/>
    protected override string LambdaName => "Generation";

    /// <inheritdoc/>
    protected override SubscriptionLambda GetSubscriptionLambda(Subscription subscription)
    {
        return subscription.ReplyToGeneration;
    }
}
