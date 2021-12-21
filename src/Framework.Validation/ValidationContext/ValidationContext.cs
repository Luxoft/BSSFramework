using System;

using Framework.Core;

namespace Framework.Validation
{
    public class ValidationContext<TSource, TValidationMap> : ValidationContextBase<TSource>, IValidationContext<TSource, TValidationMap>
        where TValidationMap : class
    {
        public ValidationContext(IValidator validator, int operationContext, TSource source, IValidationState parentState, TValidationMap map, IDynamicSource extendedValidationData)
            : base(validator, operationContext, source, parentState)
        {
            if (map == null) throw new ArgumentNullException(nameof(map));
            if (validator == null) throw new ArgumentNullException(nameof(validator));
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (extendedValidationData == null) throw new ArgumentNullException(nameof(extendedValidationData));

            this.Map = map;
            this.ExtendedValidationData = extendedValidationData;
        }


        public TValidationMap Map { get; }


        public IDynamicSource ExtendedValidationData { get; }
    }
}