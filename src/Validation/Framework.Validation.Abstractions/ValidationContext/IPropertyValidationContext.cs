using Framework.Validation.Map;

// ReSharper disable once CheckNamespace
namespace Framework.Validation;

public interface IPropertyValidationContext<out TSource, out TProperty> : IValidationContext<TSource, IPropertyValidationMap>
{
    TProperty Value { get; }
}
