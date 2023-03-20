using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SampleSystem.IntegrationTests.NH;

public static class DialectExtensions
{
    public static bool FullTextContains(this string source, string pattern)
    {
        throw new InvalidOperationException("This method for db layer, not for application layer");
    }

    public static MethodInfo GetPropetyFullTextContainsMethodInfo()
    {
        Expression<Func<string, bool>> func = z => z.FullTextContains(z);

        var result = ((MethodCallExpression)func.Body).Method;

        return result;
    }
}
