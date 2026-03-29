using System.Runtime.Serialization;

using Framework.OData.QueryLanguage.Constant.Base;

namespace Framework.OData.QueryLanguage.Constant;

[DataContract]
public record EnumConstantExpression(string Value) : ConstantExpression<string>(Value)
{
    public EnumConstantExpression(Enum value)
        : this(value.ToString())
    {
    }

    public override string ToString() => $"\"{this.Value}\"";
}
