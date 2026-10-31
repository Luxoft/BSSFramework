namespace Framework.Relations;

/// <summary>
/// Flag indicating whether the property is a detail of the object
/// If a property is a detail, it is saved to the database together with saving the master object,
/// and also in Rich DTO it is Rich as well (accordingly it disappears from Full DTO).
///
/// If a collection is marked with the DetailRole(false) attribute, it is not a detail of the object and is not saved to the db when the master object changes.
///
/// <see href="confluence/display/IADFRAME/DetailRoleAttribute"/>
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.Class)]
public class DetailRoleAttribute(DetailRole role) : Attribute
{
    public DetailRoleAttribute(bool yesOrNo)
            : this(yesOrNo ? DetailRole.Yes : DetailRole.No)
    {
    }

    public DetailRole Role { get; } = role;

    public bool HasValue(bool value) => value ? this.Role == DetailRole.Yes : this.Role == DetailRole.No;
}
