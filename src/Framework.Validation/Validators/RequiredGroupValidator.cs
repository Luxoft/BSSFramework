using System.Linq.Expressions;
using System.Reflection;

using CommonFramework;

using Framework.Core;
using Framework.Restriction;

namespace Framework.Validation;

public class RequiredGroupValidator : IClassValidator
{
    private readonly RequiredGroupValidatorMode _mode;

    private readonly string _groupKey;


    public RequiredGroupValidator(RequiredGroupValidatorMode mode, string groupKey)
    {
        if (!Enum.IsDefined(typeof (RequiredGroupValidatorMode), mode)) throw new ArgumentOutOfRangeException(nameof(mode));

        this._mode = mode;
        this._groupKey = groupKey;
    }

    public IClassValidator GetActual(IServiceProvider serviceProvider, Type type)
    {
        var uniProperties = typeof(TSource).GetUniqueElementPropeties(this._groupKey, true);

        var uniqueElementString = uniProperties.GetUniqueElementString(false);

        var propertyValidators = uniProperties.ToDictionary(property => property.Name, GetPropertyRequiredValidator<TSource>);

        return new RequiredGroupValidator<TSource>(this._mode, propertyValidators, uniqueElementString);
    }
    private static Func<TSource, bool> GetPropertyRequiredValidator<TSource>(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        return new Func<Expression<Func<TSource, object>>, Func<TSource, bool>>(GetPropertyRequiredValidator)
               .CreateGenericMethod(typeof(TSource), property.PropertyType)
               .Invoke<Func<TSource, bool>>(null, property.ToGetLambdaExpression());
    }

    private static Func<TSource, bool> GetPropertyRequiredValidator<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyExpr)
    {
        if (propertyExpr == null) throw new ArgumentNullException(nameof(propertyExpr));

        var isValidExpr = from propertyValue in propertyExpr

                          select RequiredHelper.IsValid(propertyValue, RequiredMode.Default);

        return isValidExpr.Compile();
    }
}

public class RequiredGroupValidator<TSource>(RequiredGroupValidatorMode mode, Dictionary<string, Func<TSource, bool>> propertyValidators, string uniqueElementString)
    : IClassValidator<TSource>
{
    public ValidationResult GetValidationResult(IClassValidationContext<TSource> validationContext)
    {
        if (validationContext == null) throw new ArgumentNullException(nameof(validationContext));

        var validState = propertyValidators.ChangeValue(f => f(validationContext.Source))
                             .Where(pair => pair.Value).ToDictionary();


        if (mode == RequiredGroupValidatorMode.OneOrNothing)
        {
            if (validState.Count > 1)
            {
                return ValidationResult.CreateError("More one of properties ({0}) initialized", uniqueElementString);
            }
            else
            {
                return ValidationResult.Success;
            }
        }
        else if (mode == RequiredGroupValidatorMode.AllOrNothing)
        {
            if (validState.IsEmpty() || validState.Count == propertyValidators.Count)
            {
                return ValidationResult.Success;
            }
            else
            {
                return ValidationResult.CreateError("All or no one of properties ({0}) must initialized", uniqueElementString);
            }
        }
        else if (!validState.Any())
        {
            return ValidationResult.CreateError("No one of properties ({0}) not initialized", uniqueElementString);
        }
        else if (mode == RequiredGroupValidatorMode.One && validState.Count > 1)
        {
            return ValidationResult.CreateError("More one of properties ({0}) initialized", uniqueElementString);
        }
        else
        {
            return ValidationResult.Success;
        }
    }
}
