namespace Framework.OData.QueryLanguage;

public static class MethodExpressionTypeExtensions
{
    public static string ToFormatString(this MethodExpressionType type) =>
        type switch
        {
            MethodExpressionType.StringStartsWith => "StartsWith",

            MethodExpressionType.StringContains => "Contains",

            MethodExpressionType.StringEndsWith => "EndsWith",

            MethodExpressionType.CollectionAny => "Any",

            MethodExpressionType.CollectionAll => "All",

            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
}
