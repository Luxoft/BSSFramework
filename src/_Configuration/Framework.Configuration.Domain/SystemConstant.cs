using Framework.Database.Mapping;
using Framework.Restriction;

namespace Framework.Configuration.Domain;

/// <summary>
/// System constant
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
    /// Constructor
    /// </summary>
    /// <param name="code">Code</param>
    /// <param name="type">Type</param>
    public SystemConstant(string code, DomainType type)
    {
        this.code = code ?? throw new ArgumentNullException(nameof(code));
        this.type = type ?? throw new ArgumentNullException(nameof(type));
    }

    /// <summary>
    /// Constant type
    /// </summary>
    [Required]
    public virtual DomainType Type
    {
        get => this.type;
        set => this.type = value;
    }

    /// <summary>
    /// Unique constant name
    /// </summary>
    [UniqueElement]
    [Required]
    public virtual string Code
    {
        get => this.code;
        set => this.code = value;
    }

    /// <summary>
    /// Constant value
    /// </summary>
    [MaxLength]
    public virtual string Value
    {
        get => this.value;
        set => this.value = value;
    }

    /// <summary>
    /// Constant description
    /// </summary>
    [MaxLength]
    public virtual string Description
    {
        get => this.description;
        set => this.description = value;
    }

    /// <summary>
    /// Flag of a manual constant change
    /// </summary>
    public virtual bool IsManual
    {
        get => this.isManual;
        set => this.isManual = value;
    }

    public override string ToString() => this.Code;
}
