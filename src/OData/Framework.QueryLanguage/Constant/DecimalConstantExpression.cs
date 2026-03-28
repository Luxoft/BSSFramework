namespace Framework.QueryLanguage;

public record DecimalConstantExpression(decimal Value) : ConstantExpression<decimal>(Value);
