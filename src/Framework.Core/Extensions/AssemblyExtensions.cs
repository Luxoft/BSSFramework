using System.Reflection;

namespace Framework.Core;

public static class AssemblyExtensions
{
    public static Assembly TryLoad(this AssemblyName assemblyName)
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
