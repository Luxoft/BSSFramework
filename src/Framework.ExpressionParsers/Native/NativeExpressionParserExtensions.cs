using System.Linq.Expressions;

using CommonFramework;
using Framework.Core;

namespace Framework.ExpressionParsers.Native;

public static class NativeExpressionParserExtensions
{
    public static Expression<TDelegate> Parse<TDelegate>(this INativeExpressionParser expressionParserFactory, string expression)
        where TDelegate : Delegate
    {
        if (expressionParserFactory == null) throw new ArgumentNullException(nameof(expressionParserFactory));
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        var delegateType = typeof(TDelegate);

        var invokeMethod = delegateType.GetMethod("Invoke")!;

        return (Expression<TDelegate>)expressionParserFactory.Parse(new NativeExpressionParsingData(new MethodTypeInfo(invokeMethod), expression));
    }

    public static INativeExpressionParser ToSafeComposite(this INativeExpressionParser[] innerParsers) => new TryFaultMixedExpressionParserFactory(innerParsers);

    private class TryFaultMixedExpressionParserFactory(INativeExpressionParser[] innerParsers) : INativeExpressionParser
    {
        public LambdaExpression Parse(NativeExpressionParsingData input)
        {
            var preResult = innerParsers.ToArray(factory => LazyHelper.Create(() => TryResult.Catch(() => factory.Parse(input))));

            var firstSuccess = preResult.FirstOrDefault(v => v.Value.IsSuccess());

            if (firstSuccess != null)
            {
                return firstSuccess.Value.GetValue();
            }

            var errors = preResult.Select(v => v.Value).GetErrors();

            throw new AggregateException(errors);
        }
    }
}
