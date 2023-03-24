using Framework.Core;

namespace Framework.Validation;

public class ClassValidationContext<TSource> : ValidationContext<TSource, IClassValidationMap>, IClassValidationContext<TSource>
{
    public ClassValidationContext(IValidator validator, int operationContext, TSource source, IValidationState parentState, IClassValidationMap<TSource> map, IDynamicSource extendedValidationData)
            : base(validator, operationContext, source, parentState, map, extendedValidationData)
    {
    }
}
