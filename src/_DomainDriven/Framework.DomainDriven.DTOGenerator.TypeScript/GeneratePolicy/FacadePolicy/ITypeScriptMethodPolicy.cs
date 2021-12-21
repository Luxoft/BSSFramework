using System.Reflection;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.Facade
{
    /// <summary>
    /// Политика фильтрации используемых методов в typescript
    /// </summary>
    public interface ITypeScriptMethodPolicy
    {
        /// <summary>
        /// Проверка на использование метода
        /// </summary>
        /// <param name="methodInfo">метод</param>
        /// <returns></returns>
        bool Used(MethodInfo methodInfo);
    }
}
