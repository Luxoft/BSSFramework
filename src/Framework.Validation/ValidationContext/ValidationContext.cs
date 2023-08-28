namespace Framework.Validation;

public class ValidationContext<TSource, TValidationMap> : ValidationContextBase<TSource>, IValidationContext<TSource, TValidationMap>
        where TValidationMap : class
{
    public ValidationContext(IValidator validator, int operationContext, TSource source, IValidationState parentState, TValidationMap map, IServiceProvider serviceProvider)
            : base(validator, operationContext, source, parentState)
    {
        if (map == null) throw new ArgumentNullException(nameof(map));
        if (validator == null) throw new ArgumentNullException(nameof(validator));
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

        this.Map = map;
        this.ServiceProvider = serviceProvider;
    }


    public TValidationMap Map { get; }


    public IServiceProvider ServiceProvider { get; }
}
