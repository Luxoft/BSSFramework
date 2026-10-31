using System.Reflection;
using System.Reflection.Emit;

using Anch.Core;

namespace Framework.Core;

/// <summary>
/// Class for automatic creation of an interface adapter by generating its implementation at runtime via Lazy{T}
/// </summary>
public class LazyInterfaceImplementTypeBuilder : InterfaceImplementTypeBuilder
{
    /// <summary>
    /// Default instance
    /// </summary>
    public static readonly LazyInterfaceImplementTypeBuilder Default =
            AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("Default_LazyInterfaceImplement"), AssemblyBuilderAccess.Run)
                           .Pipe(assemblyBuilder => assemblyBuilder.DefineDynamicModule("Default_LazyInterfaceImplement.dll"))
                           .Pipe(moduleBuilder => new LazyInterfaceImplementTypeBuilder(moduleBuilder));

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="moduleBuilder">Module in which the anonymous type will be generated</param>
    public LazyInterfaceImplementTypeBuilder(ModuleBuilder moduleBuilder)
            : base(moduleBuilder, typeof(Lazy<>))
    {
    }
}
