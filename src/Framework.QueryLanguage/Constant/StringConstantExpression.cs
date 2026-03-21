namespace Framework.QueryLanguage;

public record StringConstantExpression(string Value) : ConstantExpression<string>(Value)
{
    public override string ToString() => $"\"{this.Value}\"";
}
