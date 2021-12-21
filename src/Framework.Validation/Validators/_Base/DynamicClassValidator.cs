using System;

using Framework.Core;

namespace Framework.Validation
{
    public abstract class DynamicClassValidator : IDynamicClassValidator
    {
        public IClassValidator GetValidator(Type type, IDynamicSource extendedValidationData)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (extendedValidationData == null) throw new ArgumentNullException(nameof(extendedValidationData));

            return new Func<IDynamicSource, IClassValidator>(this.GetValidator<object>)
                  .CreateGenericMethod(type)
                  .Invoke<IClassValidator>(this, extendedValidationData);
        }

        protected abstract IClassValidator GetValidator<TSource>(IDynamicSource extendedValidationData);
    }
}