using System.Runtime.Serialization;

using Framework.QueryLanguage;

namespace Framework.OData;

public record SelectOrder(LambdaExpression Path, OrderType OrderType);
