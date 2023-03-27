namespace Framework.QueryLanguage;

public static class MethodExpressionTypeExtensions
{
    public static string ToFormatString(this MethodExpressionType type)
    {
        switch (type)
        {
            case MethodExpressionType.StringStartsWith:
                return "StartsWith";

            case MethodExpressionType.StringContains:
                return "Contains";

            case MethodExpressionType.PeriodContains:
                return "Contains";

            case MethodExpressionType.PeriodIsIntersected:
                return "IsIntersected";

            case MethodExpressionType.StringEndsWith:
                return "EndsWith";

            case MethodExpressionType.CollectionAny:
                return "Any";

            case MethodExpressionType.CollectionAll:
                return "All";

            default:
                throw new ArgumentOutOfRangeException(nameof(type));
        }
    }
}
