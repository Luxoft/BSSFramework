using System;

namespace Framework.DomainDriven;

public class MappingPriorityAttribute : Attribute
{
    public MappingPriorityAttribute(int value)
    {
        this.Value = value;
    }


    public int Value { get; private set; }
}
