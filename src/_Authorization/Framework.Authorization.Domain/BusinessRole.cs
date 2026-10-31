using Framework.Relations;
using Framework.Restriction;

namespace Framework.Authorization.Domain;

/// <summary>
/// Set of security operations granted to a principal together with the context of their application
/// </summary>
[UniqueGroup]
public class BusinessRole : BaseDirectory
{
    private readonly ICollection<Permission> permissions = new List<Permission>();

    private string description = "";

    /// <summary>
    /// Collection of principal permissions granted for a single business role
    /// </summary>
    [DetailRole(false)]
    public virtual IEnumerable<Permission> Permissions => this.permissions;

    /// <summary>
    /// Business role description
    /// </summary>
    public virtual string Description
    {
        get => this.description;
        set => this.description = value;
    }
}
