namespace Framework.QueryLanguage;

public record BooleanConstantExpression(bool Value) : ConstantExpression<bool>(Value)
{
    public static BooleanConstantExpression True { get; } = new(true);

    public static BooleanConstantExpression False { get; } = new(false);
}
