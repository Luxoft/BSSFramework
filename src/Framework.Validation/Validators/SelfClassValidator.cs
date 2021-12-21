using System;

using JetBrains.Annotations;

namespace Framework.Validation
{
    public class SelfClassValidator<TSource> : IClassValidator<TSource>
        where TSource : IClassValidator<TSource>
    {
        public ValidationResult GetValidationResult([NotNull] IClassValidationContext<TSource> validationContext)
        {
            if (validationContext == null) throw new ArgumentNullException(nameof(validationContext));

            return validationContext.Source.GetValidationResult(validationContext);
        }
    }
}
