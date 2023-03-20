using System;

namespace Framework.Persistent;

[Obsolete("Does not used anymore, will be removed in future version", false)]
public class PersistentDomainObjectAttribute : Attribute
{
    public readonly Type Type;

    public PersistentDomainObjectAttribute(Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        this.Type = type;
    }
}
