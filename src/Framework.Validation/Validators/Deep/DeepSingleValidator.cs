using Framework.Core;

namespace Framework.Validation
{
    /// <summary>
    /// ���������� ���������
    /// </summary>
    /// <typeparam name="TSource">��� �������� �������</typeparam>
    /// <typeparam name="TProperty">��� ��������</typeparam>
    public class DeepSingleValidator<TSource, TProperty> : IPropertyValidator<TSource, TProperty>
    {
        /// <inheritdoc />
        public ValidationResult GetValidationResult(IPropertyValidationContext<TSource, TProperty> validationContext)
        {
            return validationContext.Value.Pipe(value => validationContext.Validator.GetValidationResult(value, validationContext.OperationContext, new ValidationState(validationContext.ParentState, validationContext.Map, validationContext.Source)));
        }
    }
}