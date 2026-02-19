namespace Framework.Validation;

public class ValidationContextBase<TSource>(IValidator validator, int operationContext, TSource source, IValidationState? parentState)
    : IValidationContextBase<TSource>
{
    /// <inheritdoc />
    public IValidator Validator { get; } = validator;

    /// <inheritdoc />
    public int OperationContext { get; } = operationContext;

    /// <inheritdoc />
    public TSource Source { get; } = source;

    /// <inheritdoc />
    public IValidationState? ParentState { get; } = parentState;
}
