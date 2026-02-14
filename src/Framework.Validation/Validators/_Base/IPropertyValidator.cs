namespace Framework.Validation;

public interface IPropertyValidator
{

}

public interface IPropertyValidator<in TSource, in TProperty> : IPropertyValidator, IElementValidator<IPropertyValidationContext<TSource, TProperty>>
{

}
