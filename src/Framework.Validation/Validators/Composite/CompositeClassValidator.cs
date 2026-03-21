namespace Framework.Validation;

public class CompositeClassValidator<TSource>(params IClassValidator<TSource>[] classValidators) : IClassValidator<TSource>
    where TSource : class
{
    private readonly IList<IClassValidator<TSource>> classValidators = classValidators.ToList();

    public ValidationResult GetValidationResult(IClassValidationContext<TSource> context) => this.classValidators.Select(validator => validator.GetValidationResult(context)).Sum();
}
