namespace Framework.QueryLanguage;

public record Int64ConstantExpression(long Value) : ConstantExpression<long>(Value);
