using System;

using JetBrains.Annotations;

namespace Framework.Core
{
    public static class TypeSourceExtensions
    {
        public static ITypeResolver<string> ToDefaultTypeResolver([NotNull] this ITypeSource typeSource)
        {
            if (typeSource == null) throw new ArgumentNullException(nameof(typeSource));

            return TypeResolverHelper.CreateDefault(typeSource);
        }
    }
}
