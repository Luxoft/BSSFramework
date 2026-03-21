namespace Framework.QueryLanguage;

public record DateTimeConstantExpression (DateTime Value) : ConstantExpression<DateTime>(Value);
