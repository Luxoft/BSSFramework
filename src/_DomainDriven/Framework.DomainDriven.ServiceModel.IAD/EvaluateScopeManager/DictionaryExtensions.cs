using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Framework.DomainDriven.ServiceModel
{
    internal static class DictionaryExtensions
    {
        public static void RegisterScope<T>([NotNull] this Dictionary<Type, object> dict, T value)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));

            dict.Add(typeof(T), value);
        }


        public static bool TryRegisterScope<T>([NotNull] this Dictionary<Type, object> dict, T value)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));

            return dict.TryAdd(typeof(T), value);
        }
    }
}
