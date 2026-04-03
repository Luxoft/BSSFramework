using System.Reflection;

namespace Framework.Core.AnonymousTypeBuilder;

public class TypeMapProperty : ITypeMapMember
{
    public TypeMapProperty(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        this.Property = property;
    }

    public PropertyInfo Property { get; }

    public virtual string Name => this.Property.Name;

    public virtual Type Type => this.Property.PropertyType;
}
