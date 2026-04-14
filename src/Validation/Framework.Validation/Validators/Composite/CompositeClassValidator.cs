namespace Framework.Validation.Validators.Composite;

public class CompositeClassValidator<TSource>(params IClassValidator<TSource>[] classValidators) : IClassValidator<TSource>
    where TSource : class
{
    private readonly List<IClassValidator<TSource>> classValidators = classValidators.ToList();

    public ValidationResult GetValidationResult(IClassValidationContext<TSource> context) => this.classValidators.Select(validator => validator.GetValidationResult(context)).Sum();
}
