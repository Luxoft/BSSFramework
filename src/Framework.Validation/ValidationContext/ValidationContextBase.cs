using System;

namespace Framework.Validation
{
    public class ValidationContextBase<TSource> : IValidationContextBase<TSource>
    {
        public ValidationContextBase(IValidator validator, int operationContext, TSource source, IValidationState parentState)
        {
            if (validator == null) throw new ArgumentNullException(nameof(validator));

            this.OperationContext = operationContext;
            this.Source = source;
            this.Validator = validator;
            this.ParentState = parentState;
        }


        /// <inheritdoc />
        public IValidator Validator { get; }

        /// <inheritdoc />
        public int OperationContext { get; }

        /// <inheritdoc />
        public TSource Source { get; }

        /// <inheritdoc />
        public IValidationState ParentState { get; }
    }
}