namespace Framework.Validation.Extensions;

public static class ValidatorExtensions
{
    public static void Validate<TSource>(this IValidator validator, TSource source, int operationContext = int.MaxValue)
            where TSource : class =>
        validator.GetValidationResult(source, operationContext).TryThrow();
}
