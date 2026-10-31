// ReSharper disable once CheckNamespace
namespace Framework.BLL.Domain.ServiceRole;

/// <summary>
/// Base attribute for a facade role
/// </summary>
public abstract class BLLServiceRoleAttribute : BLLRoleAttribute
{
    /// <summary>
    /// Constructor
    /// </summary>
    protected BLLServiceRoleAttribute()
    {
    }

    /// <summary>
    /// Manual implementation of a facade method
    /// </summary>
    public bool CustomImplementation { get; set; }
}
