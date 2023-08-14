using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Restriction;

using JetBrains.Annotations;

namespace Framework.Configuration.Domain;

/// <summary>
/// Константа системы
/// </summary>
[BLLViewRole, BLLSaveRole(AllowCreate = false)]
[ConfigurationViewDomainObject(ConfigurationSecurityOperationCode.SystemConstantView)]
[ConfigurationEditDomainObject(ConfigurationSecurityOperationCode.SystemConstantEdit)]
[UniqueGroup]
[NotAuditedClass]
[DomainType("{42C47133-A8C5-4E8E-9D46-385038BFE2B9}")]
public class SystemConstant :
        AuditPersistentDomainObjectBase,
        ICodeObject,
        ITypeObject<DomainType>,
        IValueObject<string>,
        IDescriptionObject
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
    public SystemConstant([NotNull] string code, [NotNull] DomainType type)
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
        get { return this.type; }
        internal protected set { this.type = value; }
    }

    /// <summary>
    /// Уникальное имя константы
    /// </summary>
    [UniqueElement]
    [VisualIdentity]
    [Required]
    public virtual string Code
    {
        get { return this.code.TrimNull(); }
        internal protected set { this.code = value.TrimNull(); }
    }

    /// <summary>
    /// Значение константы
    /// </summary>
    [MaxLength]
    public virtual string Value
    {
        get { return this.value.TrimNull(); }
        set { this.value = value.TrimNull(); }
    }

    /// <summary>
    /// Описание константы
    /// </summary>
    [MaxLength]
    public virtual string Description
    {
        get { return this.description.TrimNull(); }
        set { this.description = value.TrimNull(); }
    }

    /// <summary>
    /// Признак изменения константы вручную
    /// </summary>
    public virtual bool IsManual
    {
        get { return this.isManual; }
        internal protected set { this.isManual = value; }
    }

    public override string ToString()
    {
        return this.Code;
    }
}
