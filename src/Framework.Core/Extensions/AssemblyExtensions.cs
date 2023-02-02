using System;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

namespace Framework.Core
{
    public static class AssemblyExtensions
    {
        public static Assembly TryLoad([NotNull] this AssemblyName assemblyName)
        {
            if (assemblyName == null) throw new ArgumentNullException(nameof(assemblyName));

            try
            {
                return Assembly.Load(assemblyName);
            }
            catch
            {
                return null;
            }
        }
    }
}
