using System;

using Framework.Core;

namespace Framework.Persistent;

[AttributeUsage(AttributeTargets.Class)]
public class TargetSystemAttribute : Attribute
{
    public TargetSystemAttribute(string id)
            : this(id, null)
    {

    }

    public TargetSystemAttribute(string id, string name)
    {
        if (id.IsDefault()) throw new ArgumentOutOfRangeException(nameof(id));

        this.Id = new Guid(id);
        this.Name = name;
    }


    public Guid Id { get; private set; }

    public string Name { get; private set; }
}
