using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;

/// <summary>
/// Фабрика процессоров лямбда-выражений.
/// </summary>
public class LambdaProcessorFactory<TBLLContext>
        where TBLLContext : class
{
    private static readonly Dictionary<Type, Func<TBLLContext, LambdaProcessor<TBLLContext>>> Processors =
            new Dictionary<Type, Func<TBLLContext, LambdaProcessor<TBLLContext>>>
            {
                    { typeof(ConditionLambdaProcessor<TBLLContext>), (c) => new ConditionLambdaProcessor<TBLLContext>(c) },
                    { typeof(GenerationLambdaProcessorTo<TBLLContext>), (c) => new GenerationLambdaProcessorTo<TBLLContext>(c) },
                    { typeof(GenerationLambdaProcessorCc<TBLLContext>), (c) => new GenerationLambdaProcessorCc<TBLLContext>(c) },
                    { typeof(GenerationLambdaProcessorReplyTo<TBLLContext>), (c) => new GenerationLambdaProcessorReplyTo<TBLLContext>(c) },
                    { typeof(SecurityItemSourceLambdaProcessor<TBLLContext>), (c) => new SecurityItemSourceLambdaProcessor<TBLLContext>(c) },
                    { typeof(AttachmentLambdaProcessor<TBLLContext>), (c) => new AttachmentLambdaProcessor<TBLLContext>(c) },
            };

    private readonly TBLLContext bllContext;

    /// <summary>Создаёт экземпляр класса <see cref="LambdaProcessorFactory"/>.</summary>
    /// <param name="bllContext">Контекст бизнес-логики.</param>
    /// <exception cref="ArgumentNullException">Аргумент
    /// bllContext
    /// или
    /// parserFactory равен null.
    /// </exception>
    public LambdaProcessorFactory([NotNull] TBLLContext bllContext)
    {
        if (bllContext == null)
        {
            throw new ArgumentNullException(nameof(bllContext));
        }

        this.bllContext = bllContext;
    }

    /// <summary>Создает процессор лямбда-выражений.</summary>
    /// <typeparam name="T">Тип процессора лямбда-выражений, который необходимо создать.</typeparam>
    /// <returns>Экземпляр созданного процессора лямбда-выражений.</returns>
    public virtual T Create<T>()
            where T : LambdaProcessor<TBLLContext>
    {
        var createProcessor = Processors[typeof(T)];
        var result = createProcessor(this.bllContext);
        return (T)result;
    }
}
