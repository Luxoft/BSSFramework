using System.Reflection;
using System.Reflection.Emit;

using Anch.Core;

namespace Framework.Core;

/// <summary>
/// Class for automatic creation of an interface adapter by generating its implementation at runtime via CallProxy{T}
/// </summary>
public class CallProxyInterfaceImplementTypeBuilder : InterfaceImplementTypeBuilder
{
    /// <summary>
    /// Default instance
    /// </summary>
    public static readonly CallProxyInterfaceImplementTypeBuilder Default =
            AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("Default_CallProxyInterfaceImplement"), AssemblyBuilderAccess.Run)
                           .Pipe(assemblyBuilder => assemblyBuilder.DefineDynamicModule("Default_CallProxyInterfaceImplement.dll"))
                           .Pipe(moduleBuilder => new CallProxyInterfaceImplementTypeBuilder(moduleBuilder));

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="moduleBuilder">Module in which the anonymous type will be generated</param>
    public CallProxyInterfaceImplementTypeBuilder(ModuleBuilder moduleBuilder)
            : base(moduleBuilder, typeof(CallProxy<>))
    {
    }
}
