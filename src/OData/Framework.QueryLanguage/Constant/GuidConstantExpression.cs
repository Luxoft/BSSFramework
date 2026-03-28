namespace Framework.QueryLanguage;

public record GuidConstantExpression(Guid Value) : ConstantExpression<Guid>(Value)
{
    public override string ToString() => $"'{this.Value}'";
}
