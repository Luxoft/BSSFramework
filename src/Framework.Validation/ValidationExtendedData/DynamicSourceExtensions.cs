using System;

using Framework.Core;

namespace Framework.Validation
{
    internal static class DynamicSourceExtensions
    {
        public static IDynamicSource TryAdd(this IDynamicSource source, IExtendedValidationDataContainer extendedValidationDataContainer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.TryAdd(extendedValidationDataContainer.Maybe(v => v.ExtendedValidationData));
        }


        private static IDynamicSource TryAdd(this IDynamicSource source, IDynamicSource otherSource)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return otherSource == null ? source : source.Add(otherSource);
        }
    }
}