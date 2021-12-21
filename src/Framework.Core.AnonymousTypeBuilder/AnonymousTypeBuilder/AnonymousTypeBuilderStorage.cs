using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Framework.Core
{
    public class AnonymousTypeBuilderStorage : IAnonymousTypeBuilderStorage
    {
        protected AnonymousTypeBuilderStorage(ModuleBuilder moduleBuilder)
        {
            if (moduleBuilder == null) throw new ArgumentNullException(nameof(moduleBuilder));

            this.ModuleBuilder = moduleBuilder;
        }

        public AnonymousTypeBuilderStorage(AssemblyBuilder assemblyBuilder)
            : this(assemblyBuilder.DefineDynamicModule(assemblyBuilder.FullName + ".dll"))
        {
        }

        public AnonymousTypeBuilderStorage(string assemblyBuilderName, AssemblyBuilderAccess assemblyBuilderAccess = AssemblyBuilderAccess.Run)
            : this(AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(assemblyBuilderName), assemblyBuilderAccess))
        {
        }

        public ModuleBuilder ModuleBuilder { get; private set; }
    }
}
