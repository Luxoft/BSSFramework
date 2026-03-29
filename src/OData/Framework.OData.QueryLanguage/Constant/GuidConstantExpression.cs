using Framework.OData.QueryLanguage.Constant.Base;

namespace Framework.OData.QueryLanguage.Constant;

public record GuidConstantExpression(Guid Value) : ConstantExpression<Guid>(Value)
{
    public override string ToString() => $"'{this.Value}'";
}
