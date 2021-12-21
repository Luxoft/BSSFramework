using System;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

namespace Framework.Core
{
    public static class AssemblyExtensions
    {
        public static Version GetVersion(this Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            return new Version(assembly.FullName.SkipWhile(z => z != ' ').SkipWhile(z => z != '=').Skip(1).TakeWhile(z => z != ',').Concat());
        }

        public static Type[] TryGetTypes(this Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            try
            {
                return assembly.GetTypes();
            }
            catch
            {
                return Type.EmptyTypes;
            }
        }

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
