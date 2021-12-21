using System;
using System.Reflection;
using System.Reflection.Emit;

using JetBrains.Annotations;

namespace Framework.Core
{
    /// <summary>
    /// Класс, для автоматического создания адаптера интерфейса посредством генерации его имплементации в рантайме через Lazy{T}
    /// </summary>
    public class LazyInterfaceImplementTypeBuilder : InterfaceImplementTypeBuilder
    {
        /// <summary>
        /// Дефолтовый инстанс
        /// </summary>
        public static readonly LazyInterfaceImplementTypeBuilder Default =
            AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("Default_LazyInterfaceImplement"), AssemblyBuilderAccess.Run)
                .Pipe(assemblyBuilder => assemblyBuilder.DefineDynamicModule("Default_LazyInterfaceImplement.dll"))
                .Pipe(moduleBuilder => new LazyInterfaceImplementTypeBuilder(moduleBuilder));

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="moduleBuilder">Модуль, где будет генерировать анонимный тип</param>
        public LazyInterfaceImplementTypeBuilder([NotNull] ModuleBuilder moduleBuilder)
             :base(moduleBuilder, typeof(Lazy<>))
        {
        }
    }
}
