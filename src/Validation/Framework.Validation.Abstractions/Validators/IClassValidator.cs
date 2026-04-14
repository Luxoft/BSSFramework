namespace Framework.Validation.Validators;

public interface IClassValidator;

public interface IClassValidator<in TSource> : IClassValidator, IElementValidator<IClassValidationContext<TSource>>;

public interface IDynamicClassValidator : IClassValidator, IDynamicClassValidatorBase;

public interface IManyPropertyDynamicClassValidator : IClassValidator, IDynamicPropertyValidatorBase;
