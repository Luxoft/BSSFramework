using System;

namespace Framework.DomainDriven.ServiceModel.IAD;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class AutoRequestPropertyAttribute : Attribute
{
    public int OrderIndex
    {
        get;
        set;
    }
}
