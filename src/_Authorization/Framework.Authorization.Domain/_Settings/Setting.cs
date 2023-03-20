namespace Framework.Authorization.Domain;

/// <summary>
/// Словарь базы "Авторизация"
/// </summary>
public class Setting : AuditPersistentDomainObjectBase
{
    private string key;

    private string value;

    /// <summary>
    /// Название словаря
    /// </summary>
    public virtual string Key
    {
        get { return this.key; }
        set { this.key = value; }
    }

    /// <summary>
    /// Значение
    /// </summary>
    public virtual string Value
    {
        get { return this.value; }
        set { this.value = value; }
    }
}
