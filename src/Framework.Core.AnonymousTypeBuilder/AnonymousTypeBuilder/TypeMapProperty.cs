using System;
using System.Reflection;

namespace Framework.Core;

public class TypeMapProperty : ITypeMapMember
{
    public TypeMapProperty(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        this.Property = property;
    }

    public PropertyInfo Property { get; private set; }

    public virtual string Name
    {
        get { return this.Property.Name; }
    }

    public virtual Type Type
    {
        get { return this.Property.PropertyType; }
    }
}
