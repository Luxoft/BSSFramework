using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Framework.Core;

namespace Framework.Security.Cryptography
{
    public static class CustomAttributeProviderExtensions
    {
        public static Enum GetCryptSystem(this ICustomAttributeProvider source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.GetCustomAttribute<CryptAttribute>().Maybe(v => v.System);
        }
    }
}