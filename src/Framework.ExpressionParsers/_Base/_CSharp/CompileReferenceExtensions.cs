using System.Reflection;

using Framework.Core;

namespace Framework.ExpressionParsers;

internal static class CompileReferenceExtensions
{
    public static IEnumerable<Type> GetCompileReferencedTypes(this IEnumerable<Type> types)
    {
        if (types == null) throw new ArgumentNullException(nameof(types));

        var graph = new HashSet<Type>();

        graph.FillCompileReferencedTypes(types.Distinct());

        return graph;
    }

    public static IEnumerable<Assembly> GetCompileReferencedAssemblies(this IEnumerable<Assembly> assemblies)
    {
        if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

        var graph = new HashSet<Assembly>();

        graph.FillCompileReferencedAssemblies(assemblies.Distinct());

        return graph;
    }


    private static void FillCompileReferencedTypes(this HashSet<Type> graph, IEnumerable<Type> types)
    {
        if (types == null) throw new ArgumentNullException(nameof(types));

        foreach (var type in types)
        {
            if (graph.Add(type))
            {
                if (type.IsGenericType)
                {
                    foreach (var argType in type.GetGenericArguments())
                    {
                        graph.FillCompileReferencedTypes(new[] { argType });
                    }
                }

                if (type.IsInterface)
                {
                    foreach (var interfaceType in type.GetInterfaces())
                    {
                        graph.FillCompileReferencedTypes(new[] { interfaceType });
                    }
                }
                else if (type.IsClass)
                {
                    foreach (var t in type.GetAllElements(t => t.BaseType))
                    {
                        graph.FillCompileReferencedTypes(new[] { t });
                    }

                    foreach (var interfaceType in type.GetInterfaces())
                    {
                        graph.FillCompileReferencedTypes(new[] { interfaceType });
                    }
                }
            }
        }

        //// add public types of fields/properties/methods?
    }

    private static void FillCompileReferencedAssemblies(this HashSet<Assembly> graph, IEnumerable<Assembly> assemblies)
    {
        if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

        foreach (var assembly in assemblies)
        {
            if (graph.Add(assembly))
            {
                foreach (var refAssemblyName in assembly.GetReferencedAssemblies())
                {
                    var startRefAssembly = refAssemblyName.TryLoad();

                    if (startRefAssembly != null)
                    {
                        graph.Add(startRefAssembly);
                        //graph.FillCompileReferencedAssemblies(new[] { startRefAssembly });
                    }
                }
            }
        }
    }


    ////private static Assembly TryLoad(this AppDomain appDomain, [NotNull] AssemblyName assemblyName)
    ////{
    ////    try
    ////    {
    ////        return appDomain.Load(assemblyName);
    ////    }
    ////    catch
    ////    {
    ////        return null;
    ////    }
    ////}
}
