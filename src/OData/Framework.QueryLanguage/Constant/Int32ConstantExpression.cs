namespace Framework.QueryLanguage;

public record Int32ConstantExpression(int Value) : ConstantExpression<int>(Value);
