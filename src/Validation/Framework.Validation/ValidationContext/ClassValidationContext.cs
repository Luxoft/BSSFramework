namespace Framework.Validation;

public class ClassValidationContext<TSource>(
    IValidator validator,
    int operationContext,
    TSource source,
    IValidationState? parentState,
    IClassValidationMap<TSource> map,
    IServiceProvider serviceProvider)
    : ValidationContext<TSource, IClassValidationMap>(validator, operationContext, source, parentState, map, serviceProvider), IClassValidationContext<TSource>;
