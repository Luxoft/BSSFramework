using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas
{
    /// <summary>
    /// Фабрика процессоров лямбда-выражений.
    /// </summary>
    public class LambdaProcessorFactory<TBLLContext>
        where TBLLContext : class
    {
        private static readonly Dictionary<Type, Func<TBLLContext, IExpressionParserFactory, LambdaProcessor<TBLLContext>>> Processors =
            new Dictionary<Type, Func<TBLLContext, IExpressionParserFactory, LambdaProcessor<TBLLContext>>>
            {
                { typeof(ConditionLambdaProcessor<TBLLContext>), (c, pf) => new ConditionLambdaProcessor<TBLLContext>(c, pf) },
                { typeof(DynamicSourceLambdaProcessor<TBLLContext>), (c, pf) => new DynamicSourceLambdaProcessor<TBLLContext>(c, pf) },
                { typeof(GenerationLambdaProcessorTo<TBLLContext>), (c, pf) => new GenerationLambdaProcessorTo<TBLLContext>(c, pf) },
                { typeof(GenerationLambdaProcessorCc<TBLLContext>), (c, pf) => new GenerationLambdaProcessorCc<TBLLContext>(c, pf) },
                { typeof(GenerationLambdaProcessorReplyTo<TBLLContext>), (c, pf) => new GenerationLambdaProcessorReplyTo<TBLLContext>(c, pf) },
                { typeof(SecurityItemSourceLambdaProcessor<TBLLContext>), (c, pf) => new SecurityItemSourceLambdaProcessor<TBLLContext>(c, pf) },
                { typeof(AttachmentLambdaProcessor<TBLLContext>), (c, pf) => new AttachmentLambdaProcessor<TBLLContext>(c, pf) },
            };

        private readonly TBLLContext bllContext;
        private readonly IExpressionParserFactory parserFactory;

        /// <summary>Создаёт экземпляр класса <see cref="LambdaProcessorFactory"/>.</summary>
        /// <param name="bllContext">Контекст бизнес-логики.</param>
        /// <param name="parserFactory">Фабрика парсеров лямбда-выражений.</param>
        /// <exception cref="ArgumentNullException">Аргумент
        /// bllContext
        /// или
        /// parserFactory равен null.
        /// </exception>
        public LambdaProcessorFactory([NotNull] TBLLContext bllContext, [NotNull] IExpressionParserFactory parserFactory)
        {
            if (bllContext == null)
            {
                throw new ArgumentNullException(nameof(bllContext));
            }

            this.bllContext = bllContext;
            this.parserFactory = parserFactory ?? throw new ArgumentNullException(nameof(parserFactory));
        }

        /// <summary>Создает процессор лямбда-выражений.</summary>
        /// <typeparam name="T">Тип процессора лямбда-выражений, который необходимо создать.</typeparam>
        /// <returns>Экземпляр созданного процессора лямбда-выражений.</returns>
        public virtual T Create<T>()
            where T : LambdaProcessor<TBLLContext>
        {
            var createProcessor = Processors[typeof(T)];
            var result = createProcessor(this.bllContext, this.parserFactory);
            return (T)result;
        }
    }
}
