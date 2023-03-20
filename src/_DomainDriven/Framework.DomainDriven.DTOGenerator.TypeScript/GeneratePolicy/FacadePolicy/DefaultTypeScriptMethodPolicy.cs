using System.Reflection;

namespace Framework.DomainDriven.DTOGenerator.TypeScript.Facade;

/// <summary>
/// Политика генерации по умолчанию
/// </summary>
public class DefaultTypeScriptMethodPolicy : ITypeScriptMethodPolicy
{
    private readonly bool allowAll;

    /// <summary>
    /// Разрешить все методы
    /// </summary>
    /// <param name="allowAll"></param>
    public DefaultTypeScriptMethodPolicy(bool allowAll)
    {
        this.allowAll = allowAll;
    }

    /// <inheritdoc />
    public bool Used(MethodInfo methodInfo) => this.allowAll;
}
