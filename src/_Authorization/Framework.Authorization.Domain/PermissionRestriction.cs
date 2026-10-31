using Framework.Relations;
using Framework.Restriction;

namespace Framework.Authorization.Domain;

/// <summary>
/// Relation between a permission and a context
/// </summary>
public class PermissionRestriction : AuditPersistentDomainObjectBase, IDetail<Permission>
{
    private Permission permission = null!;

    private SecurityContextType securityContextType = null!;

    private Guid securityContextId;

    public PermissionRestriction()
    {
    }

    /// <summary>
    /// The constructor creates a context with a reference to a permission
    /// </summary>
    /// <param name="permission">Permission</param>
    public PermissionRestriction(Permission permission)
    {
        this.permission = permission ?? throw new ArgumentNullException(nameof(permission));
        this.permission.AddDetail(this);
    }

    /// <summary>
    /// Permission by the context+permission combination
    /// </summary>
    public virtual Permission Permission
    {
        get => this.permission;
        set => this.permission = value;
    }

    /// <summary>
    /// Computed domain type for the context+permission combination
    /// </summary>
    [Required]
    [UniqueElement]
    public virtual SecurityContextType SecurityContextType
    {
        get => this.securityContextType;
        set => this.securityContextType = value;
    }

    [Required]
    [UniqueElement]
    public virtual Guid SecurityContextId { get => this.securityContextId; set => this.securityContextId = value; }

    Permission IDetail<Permission>.Master => this.Permission;
}
