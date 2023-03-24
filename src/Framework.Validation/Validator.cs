using System;

using Framework.Core;

namespace Framework.Validation;

public abstract class ValidatorBase : IValidator
{
    protected ValidatorBase()
    {

    }


    public abstract ValidationResult GetValidationResult<TSource>(TSource source, int operationContext = int.MaxValue, IValidationState ownerState = null);



    public static readonly IValidator Success = new SuccessValidator();


    private class SuccessValidator : ValidatorBase
    {
        public override ValidationResult GetValidationResult<TSource>(TSource source, int operationContext = int.MaxValue, IValidationState ownerState = null)
        {
            return ValidationResult.Success;
        }
    }
}

public class Validator : ValidatorBase
{
    private readonly ValidatorCompileCache _cache;


    public Validator(ValidatorCompileCache cache)
    {
        if (cache == null) throw new ArgumentNullException(nameof(cache));

        this._cache = cache;
    }


    public override ValidationResult GetValidationResult<TSource>(TSource source, int operationContext = int.MaxValue, IValidationState ownerState = null)
    {
        return this._cache.GetValidationResult(new ValidationContextBase<TSource>(this, operationContext, source, ownerState));
    }


    public static readonly Validator Default = new Validator(ValidationMap.Default.ToCompileCache());
}
