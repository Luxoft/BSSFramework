using Framework.OData.QueryLanguage.Constant.Base;

namespace Framework.OData.QueryLanguage.Constant;

public record Int32ConstantExpression(int Value) : ConstantExpression<int>(Value);
