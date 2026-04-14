// ReSharper disable once CheckNamespace
namespace Framework.BLL.Domain.ServiceRole;

/// <summary>
/// Базовый атрибут для фасадной роли
/// </summary>
public abstract class BLLServiceRoleAttribute : BLLRoleAttribute
{
    /// <summary>
    /// Конструктор
    /// </summary>
    protected BLLServiceRoleAttribute()
    {
    }

    /// <summary>
    /// Ручная имплементация фасадного метода
    /// </summary>
    public bool CustomImplementation { get; set; }
}
