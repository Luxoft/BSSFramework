namespace Framework.Validation;

public interface IClassValidator
{
    IClassValidator GetActual(IServiceProvider serviceProvider, Type type) => this;
}

public interface IClassValidator<in TSource> : IClassValidator, IElementValidator<IClassValidationContext<TSource>>;
