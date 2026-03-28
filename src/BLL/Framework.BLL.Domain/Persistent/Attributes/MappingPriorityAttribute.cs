namespace Framework.BLL.Domain.Persistent.Attributes;

public class MappingPriorityAttribute(int value) : Attribute
{
    public int Value { get; private set; } = value;
}
