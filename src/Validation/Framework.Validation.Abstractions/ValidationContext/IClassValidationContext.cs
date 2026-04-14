// ReSharper disable once CheckNamespace
namespace Framework.Validation;

public interface IClassValidationContext<out TSource> : IValidationContext<TSource, IClassValidationMap>;
