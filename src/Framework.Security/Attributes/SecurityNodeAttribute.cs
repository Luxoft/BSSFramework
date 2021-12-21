using System;

namespace Framework.Security
{
    /// <summary>
    /// Атрибут для описания универсального пути безопасности (для проекций)
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class SecurityNodeAttribute : Attribute
    {
    }
}
