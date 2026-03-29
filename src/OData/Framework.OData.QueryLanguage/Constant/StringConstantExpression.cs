using Framework.OData.QueryLanguage.Constant.Base;

namespace Framework.OData.QueryLanguage.Constant;

public record StringConstantExpression(string Value) : ConstantExpression<string>(Value)
{
    public override string ToString() => $"\"{this.Value}\"";
}
