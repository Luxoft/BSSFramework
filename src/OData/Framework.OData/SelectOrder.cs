using Framework.OData.QueryLanguage;

namespace Framework.OData;

public record SelectOrder(LambdaExpression Path, OrderType OrderType);
