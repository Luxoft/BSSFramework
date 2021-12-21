using System;
using System.Collections.Generic;

namespace Framework.Validation
{
    /// <summary>
    /// Внутренняя валидация коллекции
    /// </summary>
    /// <typeparam name="TSource">Тип текущего объекта</typeparam>
    /// <typeparam name="TProperty">Тип свойства</typeparam>
    /// <typeparam name="TElement">Тип элемента свойства</typeparam>
    public class DeepCollectionValidator<TSource, TProperty, TElement> : IPropertyValidator<TSource, TProperty>
        where TProperty : IEnumerable<TElement>
    {
        /// <inheritdoc />
        public ValidationResult GetValidationResult(IPropertyValidationContext<TSource, TProperty> validationContext)
        {
            return validationContext.Value.Sum(value => validationContext.Validator.GetValidationResult(value, validationContext.OperationContext, new ValidationState(validationContext.ParentState, validationContext.Map, validationContext.Source)));
        }
    }
}