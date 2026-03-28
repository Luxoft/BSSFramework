using System.Runtime.Serialization;

namespace Framework.QueryLanguage;

[DataContract]
public record EnumConstantExpression(string Value) : ConstantExpression<string>(Value)
{
    public EnumConstantExpression(Enum value)
        : this(value.ToString())
    {
    }

    public override string ToString() => $"\"{this.Value}\"";
}
