using System.Reflection;
using System.Reflection.Emit;

namespace Framework.Core;

/// <summary>
/// Класс, для автоматического создания адаптера интерфейса посредством генерации его имплементации в рантайме через CallProxy{T}
/// </summary>
public class CallProxyInterfaceImplementTypeBuilder : InterfaceImplementTypeBuilder
{
    /// <summary>
    /// Дефолтовый инстанс
    /// </summary>
    public static readonly CallProxyInterfaceImplementTypeBuilder Default =
            AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("Default_CallProxyInterfaceImplement"), AssemblyBuilderAccess.Run)
                           .Pipe(assemblyBuilder => assemblyBuilder.DefineDynamicModule("Default_CallProxyInterfaceImplement.dll"))
                           .Pipe(moduleBuilder => new CallProxyInterfaceImplementTypeBuilder(moduleBuilder));

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="moduleBuilder">Модуль, где будет генерировать анонимный тип</param>
    public CallProxyInterfaceImplementTypeBuilder(ModuleBuilder moduleBuilder)
            : base(moduleBuilder, typeof(CallProxy<>))
    {
    }
}
