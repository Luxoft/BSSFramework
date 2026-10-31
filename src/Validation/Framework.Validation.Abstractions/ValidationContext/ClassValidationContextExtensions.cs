using Framework.Validation.Map;

// ReSharper disable once CheckNamespace
namespace Framework.Validation;

public static class ClassValidationContextExtensions
{
    /// <summary>
    /// Getting the validation object type name
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="context"></param>
    /// <returns></returns>
    public static string GetSourceTypeName<TSource>(this IClassValidationContext<TSource> context)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));

        return context.Map.TypeName;
    }

    /// <summary>
    ///     Converts the source IClassValidationContext to an IClassValidationContext of TExpectedSource.
    /// </summary>
    /// <typeparam name="TBaseSource">Source context type.</typeparam>
    /// <typeparam name="TExpectedSource">Resulting context type.</typeparam>
    /// <param name="context">Source context.</param>
    /// <returns>Conversion result.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="context" /> is null.</exception>
    public static IClassValidationContext<TExpectedSource> Box<TBaseSource, TExpectedSource>(
        this IClassValidationContext<TBaseSource> context)
        where TBaseSource : TExpectedSource
    {
        if (context is null) throw new ArgumentNullException(nameof(context));

        return context.Cast<TBaseSource, TExpectedSource>(v => v);
    }

    /// <summary>
    ///     Converts the source IClassValidationContext to an IClassValidationContext of TExpectedSource.
    /// </summary>
    /// <typeparam name="TBaseSource">Source context type.</typeparam>
    /// <typeparam name="TExpectedSource">Resulting context type.</typeparam>
    /// <param name="context">Source context.</param>
    /// <param name="convertSource">Conversion function.</param>
    /// <returns>Instance of <see cref="IClassValidationContext{TSource}"/></returns>
    /// <exception cref="ArgumentNullException">Argument
    /// <paramref name="context"/>
    /// or
    /// <paramref name="convertSource"/> is null.
    /// </exception>
    public static IClassValidationContext<TExpectedSource> Cast<TBaseSource, TExpectedSource>(
        this IClassValidationContext<TBaseSource> context,
        Func<TBaseSource, TExpectedSource> convertSource)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));
        if (convertSource is null) throw new ArgumentNullException(nameof(convertSource));

        return new BoxedClassValidationContext<TBaseSource, TExpectedSource>(context, convertSource);
    }

    private class BoxedClassValidationContext<TBaseSource, TExpectedSource> : IClassValidationContext<TExpectedSource>
    {
        public BoxedClassValidationContext(IClassValidationContext<TBaseSource> baseContext, Func<TBaseSource, TExpectedSource> convertSource)
        {
            if (baseContext is null) throw new ArgumentNullException(nameof(baseContext));
            if (convertSource is null) throw new ArgumentNullException(nameof(convertSource));

            this.Validator = baseContext.Validator;
            this.OperationContext = baseContext.OperationContext;
            this.Source = convertSource(baseContext.Source);
            this.Map = baseContext.Map;
            this.ServiceProvider = baseContext.ServiceProvider;
            this.ParentState = baseContext.ParentState;
        }


        public IValidator Validator { get; }

        public int OperationContext { get; }

        public IValidationState? ParentState { get; }

        public TExpectedSource Source { get; }

        public IClassValidationMap Map { get; }

        public IServiceProvider ServiceProvider { get; }
    }
}
