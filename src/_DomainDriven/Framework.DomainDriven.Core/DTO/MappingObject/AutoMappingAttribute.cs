using System;

namespace Framework.DomainDriven;

public class AutoMappingAttribute : Attribute
{
    public AutoMappingAttribute(bool enabled)
    {
        this.Enabled = enabled;
    }


    public bool Enabled { get; }
}
