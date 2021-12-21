using System;

using Framework.Core;

namespace Framework.Validation
{
    public class PropertyValidationContext<TSource, TProperty> : ValidationContext<TSource, IPropertyValidationMap>, IPropertyValidationContext<TSource, TProperty>
    {
        public PropertyValidationContext(IValidator validator, int operationContext, TSource source, IValidationState parentState, IPropertyValidationMap<TSource, TProperty> map, IDynamicSource extendedValidationData, TProperty value)
            : base(validator, operationContext, source, parentState, map, extendedValidationData)
        {
            this.Value = value;
        }


        public TProperty Value { get; }
    }
}