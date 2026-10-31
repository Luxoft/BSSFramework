using Framework.Core;
using Framework.Database.Mapping;
using Framework.Relations;
using Framework.Restriction;

namespace Framework.Authorization.Domain;

/// <summary>
/// Principal's right to perform certain actions in the system
/// </summary>
/// <remarks>
/// Permissions can be granted within contexts
/// </remarks>
/// <seealso cref="SecurityContextType"/>
[System.Diagnostics.DebuggerDisplay("Principal={Principal.Name}, Role={Role.Name}")]
public class Permission : AuditPersistentDomainObjectBase,

                          IDetail<Principal>,

                          IMaster<PermissionRestriction>,

                          IMaster<Permission>,

                          IDetail<Permission>,

                          IPeriodObject
{
    private readonly ICollection<PermissionRestriction> restrictions = new List<PermissionRestriction>();

    private readonly ICollection<Permission> delegatedTo = new List<Permission>();

    private Principal principal = null!;

    private Permission? delegatedFrom;

    private BusinessRole role = null!;

    private Period period = Period.Eternity;

    private string comment = "";

    public Permission()
    {
    }

    /// <summary>
    /// The constructor creates a permission with a reference to a principal
    /// </summary>
    /// <param name="principal">principal</param>
    public Permission(Principal principal)
    {
        this.principal = principal ?? throw new ArgumentNullException(nameof(principal));
        this.principal.AddDetail(this);
    }

    /// <summary>
    /// The constructor creates a delegated permission for a principal
    /// </summary>
    /// <param name="principal">Principal</param>
    /// <param name="delegatedFrom">Permission from which delegation occurs</param>
    public Permission(Principal principal, Permission delegatedFrom)
        : this(principal)
    {
        this.delegatedFrom = delegatedFrom ?? throw new ArgumentNullException(nameof(delegatedFrom));
        this.delegatedFrom.AddDetail(this);
    }

    /// <summary>
    /// Collection of permission contexts
    /// </summary>
    [UniqueGroup]
    public virtual IEnumerable<PermissionRestriction> Restrictions => this.restrictions;

    /// <summary>
    /// Collection of permissions to which this permission was delegated
    /// </summary>
    [Mapping(CascadeMode = CascadeMode.Enabled)]
    public virtual IEnumerable<Permission> DelegatedTo => this.delegatedTo;

    /// <summary>
    /// Permission from which this permission was delegated
    /// </summary>
    public virtual Permission? DelegatedFrom { get => this.delegatedFrom; set => this.delegatedFrom = value; }

    /// <summary>
    /// Permission validity period
    /// </summary>
    public virtual Period Period { get => this.period.Round(); set => this.period = value.Round(); }

    /// <summary>
    /// Principal to which this permission belongs
    /// </summary>
    public virtual Principal Principal { get => this.principal; set => this.principal = value; }

    /// <summary>
    /// Business role contained in the permission
    /// </summary>
    [Required]
    public virtual BusinessRole Role { get => this.role; set => this.role = value; }

    /// <summary>
    /// Permission comment
    /// </summary>
    [MaxLength]
    public virtual string Comment { get => this.comment; set => this.comment = value; }

    ICollection<PermissionRestriction> IMaster<PermissionRestriction>.Details => (ICollection<PermissionRestriction>)this.Restrictions;

    Principal IDetail<Principal>.Master => this.Principal;

    ICollection<Permission> IMaster<Permission>.Details => (ICollection<Permission>)this.DelegatedTo;

    Permission? IDetail<Permission>.Master => this.DelegatedFrom;
}
