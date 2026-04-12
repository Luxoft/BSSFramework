using Framework.Core;
using Framework.Database.Mapping;
using Framework.Restriction;

namespace Framework.Configuration.Domain;

/// <summary>
/// Константа системы
/// </summary>
[UniqueGroup]
[NotAuditedClass]
public class SystemConstant : AuditPersistentDomainObjectBase
{
    private DomainType type;


    private string code;

    private string value;

    private string description;

    private bool isManual;

    public SystemConstant()
    {
    }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="code">Код</param>
    /// <param name="type">Тип</param>
    public SystemConstant(string code, DomainType type)
    {
        if (code == null) throw new ArgumentNullException(nameof(code));
        if (type == null) throw new ArgumentNullException(nameof(type));

        this.code = code;
        this.type = type;
    }

    /// <summary>
    /// Тип константы
    /// </summary>
    [Required]
    public virtual DomainType Type
    {
        get => this.type;
        set => this.type = value;
    }

    /// <summary>
    /// Уникальное имя константы
    /// </summary>
    [UniqueElement]
    [Required]
    public virtual string Code
    {
        get => this.code.TrimNull();
        set => this.code = value.TrimNull();
    }

    /// <summary>
    /// Значение константы
    /// </summary>
    [MaxLength]
    public virtual string Value
    {
        get => this.value.TrimNull();
        set => this.value = value.TrimNull();
    }

    /// <summary>
    /// Описание константы
    /// </summary>
    [MaxLength]
    public virtual string Description
    {
        get => this.description.TrimNull();
        set => this.description = value.TrimNull();
    }

    /// <summary>
    /// Признак изменения константы вручную
    /// </summary>
    public virtual bool IsManual
    {
        get => this.isManual;
        set => this.isManual = value;
    }

    public override string ToString() => this.Code;
}
