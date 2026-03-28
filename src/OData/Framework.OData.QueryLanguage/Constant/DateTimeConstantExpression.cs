using Framework.OData.QueryLanguage.Constant.Base;

namespace Framework.OData.QueryLanguage.Constant;

public record DateTimeConstantExpression (DateTime Value) : ConstantExpression<DateTime>(Value);
