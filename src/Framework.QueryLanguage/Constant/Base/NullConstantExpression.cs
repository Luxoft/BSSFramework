namespace Framework.QueryLanguage;

public record NullConstantExpression : ConstantExpression
{
    public override object? GetRawValue() => null;

    public override string ToString() => "null";


    public static readonly NullConstantExpression Value = new();
}
