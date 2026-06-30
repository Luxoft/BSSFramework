using System.Reflection;

namespace Framework.Core.AnonymousTypeBuilder;

public class TypeMapProperty(PropertyInfo property) : ITypeMapMember
{
    public PropertyInfo Property { get; } = property ?? throw new ArgumentNullException(nameof(property));

    public virtual string Name => this.Property.Name;

    public virtual Type Type => this.Property.PropertyType;
}

