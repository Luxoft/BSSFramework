using Framework.Core;

namespace Framework.Validation;

public interface IClassValidationContext<out TSource> : IValidationContext<TSource, IClassValidationMap>
{
}

public static class ClassValidationContextExtensions
{
    /// <summary>
    /// Получение имени типа объекта валидации
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string GetSourceTypeName<TSource>(this IClassValidationContext<TSource> context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        return context.Map.TypeName;
    }

    /// <summary>
    ///     Выполняет преобразование исходного IClassValidationContext к типу IClassValidationContext от TExpectedSource.
    /// </summary>
    /// <typeparam name="TBaseSource">Тип исходного контекста.</typeparam>
    /// <typeparam name="TExpectedSource">Тип результирующего контекста.</typeparam>
    /// <param name="context">Исходный контекст.</param>
    /// <returns>Результат преобразования.</returns>
    /// <exception cref="ArgumentNullException">Аргумент <paramref name="context" /> равен null.</exception>
    public static IClassValidationContext<TExpectedSource> Box<TBaseSource, TExpectedSource>(
            this IClassValidationContext<TBaseSource> context)
            where TBaseSource : TExpectedSource
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        return context.Cast<TBaseSource, TExpectedSource>(v => v);
    }

    /// <summary>
    ///     Выполняет преобразование исходного IClassValidationContext к типу IClassValidationContext от TExpectedSource.
    /// </summary>
    /// <typeparam name="TBaseSource">Тип исходного контекста.</typeparam>
    /// <typeparam name="TExpectedSource">Тип результирующего контекста.</typeparam>
    /// <param name="context">Исходный контекст.</param>
    /// <param name="convertSource">Функция преобразования.</param>
    /// <returns>Экземпляр <see cref="IClassValidationContext{TSource}"/></returns>
    /// <exception cref="ArgumentNullException">Аргумент
    /// <paramref name="context"/>
    /// или
    /// <paramref name="convertSource"/> равен null.
    /// </exception>
    public static IClassValidationContext<TExpectedSource> Cast<TBaseSource, TExpectedSource>(
            this IClassValidationContext<TBaseSource> context,
            Func<TBaseSource, TExpectedSource> convertSource)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (convertSource == null) throw new ArgumentNullException(nameof(convertSource));

        return new BoxedClassValidationContext<TBaseSource, TExpectedSource>(context, convertSource);
    }

    private class BoxedClassValidationContext<TBaseSource, TExpectedSource> : IClassValidationContext<TExpectedSource>
    {
        public BoxedClassValidationContext(IClassValidationContext<TBaseSource> baseContext, Func<TBaseSource, TExpectedSource> convertSource)
        {
            if (baseContext == null) throw new ArgumentNullException(nameof(baseContext));
            if (convertSource == null) throw new ArgumentNullException(nameof(convertSource));

            this.Validator = baseContext.Validator;
            this.OperationContext = baseContext.OperationContext;
            this.Source = convertSource(baseContext.Source);
            this.Map = baseContext.Map;
            this.ServiceProvider = baseContext.ServiceProvider;
            this.ParentState = baseContext.ParentState;
        }


        public IValidator Validator { get; }

        public int OperationContext { get; }

        public IValidationState ParentState { get; }

        public TExpectedSource Source { get; }

        public IClassValidationMap Map { get; }

        public IServiceProvider ServiceProvider { get; }
    }
}
