using Framework.OData.QueryLanguage.Constant.Base;

namespace Framework.OData.QueryLanguage.Constant;

public record DecimalConstantExpression(decimal Value) : ConstantExpression<decimal>(Value);
