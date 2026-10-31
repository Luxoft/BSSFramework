using Framework.Validation.Map;

// ReSharper disable once CheckNamespace
namespace Framework.Validation;
/// <summary>
/// Validation context
/// </summary>
public interface IValidationContextBase : IValidatorContainer<IValidator>, IOperationContextData
{
    /// <summary>
    /// Top (by stack) validation state
    /// </summary>
    IValidationState? ParentState { get; }
}

/// <summary>
/// Validation context
/// </summary>
/// <typeparam name="TSource">Object type</typeparam>
public interface IValidationContextBase<out TSource> : IValidationContextBase
{
    /// <summary>
    /// Validated object
    /// </summary>
    TSource Source { get; }
}

/// <summary>
/// Validation context
/// </summary>
/// <typeparam name="TSource">Object type</typeparam>
/// <typeparam name="TValidationMap">Metadata</typeparam>
public interface IValidationContextBase<out TSource, out TValidationMap> : IValidationContextBase<TSource>
{
    TValidationMap Map { get; }
}
