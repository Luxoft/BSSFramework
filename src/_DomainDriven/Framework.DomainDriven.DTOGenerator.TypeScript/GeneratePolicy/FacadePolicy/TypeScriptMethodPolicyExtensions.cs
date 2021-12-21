using System;
using System.Reflection;

using JetBrains.Annotations;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.Facade
{
    /// <summary>
    /// Расширения для работы с политиками клиентских фасадов
    /// </summary>
    public static class TypeScriptMethodPolicyExtensions
    {
        /// <summary>
        /// Добавленеи политики
        /// </summary>
        /// <param name="policy">Базовая политика</param>
        /// <param name="otherPolicy">Другая политика</param>
        /// <returns></returns>
        public static ITypeScriptMethodPolicy Add([NotNull] this ITypeScriptMethodPolicy policy, [NotNull] ITypeScriptMethodPolicy otherPolicy)
        {
            if (policy == null) { throw new ArgumentNullException(nameof(policy)); }
            if (otherPolicy == null) { throw new ArgumentNullException(nameof(otherPolicy)); }

            return new FuncTypeScriptMethodPolicy(method => policy.Used(method) || otherPolicy.Used(method));
        }

        /// <summary>
        /// Исключение политики
        /// </summary>
        /// <param name="policy">Базовая политика</param>
        /// <param name="otherPolicy">Другая политика</param>
        /// <returns></returns>
        public static ITypeScriptMethodPolicy Except([NotNull] this ITypeScriptMethodPolicy policy, [NotNull] ITypeScriptMethodPolicy otherPolicy)
        {
            if (policy == null) { throw new ArgumentNullException(nameof(policy)); }
            if (otherPolicy == null) { throw new ArgumentNullException(nameof(otherPolicy)); }

            return new FuncTypeScriptMethodPolicy(method => policy.Used(method) && !otherPolicy.Used(method));
        }

        private class FuncTypeScriptMethodPolicy : ITypeScriptMethodPolicy
        {
            private readonly Func<MethodInfo, bool> func;

            public FuncTypeScriptMethodPolicy([NotNull] Func<MethodInfo, bool> func)
            {
                this.func = func ?? throw new ArgumentNullException(nameof(func));
            }


            public bool Used(MethodInfo methodInfo) => this.func(methodInfo);
        }
    }
}
