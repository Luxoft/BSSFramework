using Framework.OData.QueryLanguage.Constant.Base;

namespace Framework.OData.QueryLanguage.Constant;

public record Int64ConstantExpression(long Value) : ConstantExpression<long>(Value);
