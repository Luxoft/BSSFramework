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
    private DomainType type = null!;


    private string code = "";

    private string value = "";

    private string description = "";

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
        this.code = code ?? throw new ArgumentNullException(nameof(code));
        this.type = type ?? throw new ArgumentNullException(nameof(type));
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
        get => this.code;
        set => this.code = value;
    }

    /// <summary>
    /// Значение константы
    /// </summary>
    [MaxLength]
    public virtual string Value
    {
        get => this.value;
        set => this.value = value;
    }

    /// <summary>
    /// Описание константы
    /// </summary>
    [MaxLength]
    public virtual string Description
    {
        get => this.description;
        set => this.description = value;
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
