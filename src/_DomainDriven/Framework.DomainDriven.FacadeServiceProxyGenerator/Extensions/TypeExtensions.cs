using System;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.DomainDriven.FacadeServiceProxyGenerator
{
    internal static class TypeExtensions
    {
        public static bool IsStrict([NotNull] this Type t)
        {
            if (t == null) { throw new ArgumentNullException(nameof(t)); }

            return t.HasAttribute<DTOFileTypeAttribute>(attr => attr.Name == DTOGenerator.FileType.StrictDTO.Name);
        }
    }
}
