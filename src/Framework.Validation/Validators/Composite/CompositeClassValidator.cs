using System.Collections.Generic;
using System.Linq;

namespace Framework.Validation
{
    public class CompositeClassValidator<TSource> : IClassValidator<TSource>
        where TSource : class
    {
        private readonly IList<IClassValidator<TSource>> _classValidators;

        public CompositeClassValidator(params IClassValidator<TSource>[] classValidators)
        {
            this._classValidators = classValidators.ToList();
        }

        public ValidationResult GetValidationResult(IClassValidationContext<TSource> context)
        {
            return this._classValidators.Select(validator => validator.GetValidationResult(context)).Sum();
        }
    }
}