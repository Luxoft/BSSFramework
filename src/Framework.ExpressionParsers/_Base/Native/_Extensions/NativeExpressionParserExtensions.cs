using System.Linq.Expressions;

using CommonFramework;

using Framework.Core;

namespace Framework.ExpressionParsers;

public static class NativeExpressionParserExtensions
{
    public static Expression<TDelegate> Parse<TDelegate>(this INativeExpressionParser expressionParserFactory, string expression)
    {
        if (expressionParserFactory == null) throw new ArgumentNullException(nameof(expressionParserFactory));
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        var delegateType = typeof(TDelegate);

        if (!typeof(Delegate).IsAssignableFrom(delegateType))
        {
            throw new ArgumentOutOfRangeException("TDelegate");
        }

        var invokeMethod = delegateType.GetMethod("Invoke");

        return (Expression<TDelegate>)expressionParserFactory.Parse(new NativeExpressionParsingData(new MethodTypeInfo(invokeMethod), expression));
    }

    public static INativeExpressionParser ToSafeComposite(this INativeExpressionParser[] innerParsers)
    {
        return new TryFaultMixedExpressionParserFactory(innerParsers);
    }

    private class TryFaultMixedExpressionParserFactory : INativeExpressionParser
    {
        private readonly INativeExpressionParser[] _innerParsers;


        public TryFaultMixedExpressionParserFactory(INativeExpressionParser[] innerParsers)
        {
            if (innerParsers == null) throw new ArgumentNullException(nameof(innerParsers));

            this._innerParsers = innerParsers;
        }


        public LambdaExpression Parse(NativeExpressionParsingData input)
        {
            var preResult = this._innerParsers.ToArray(factory => LazyHelper.Create(() => TryResult.Catch(() => factory.Parse(input))));


            var firstSucces = preResult.FirstOrDefault(v => v.Value.IsSuccess());

            if (firstSucces != null)
            {
                return firstSucces.Value.GetValue();
            }

            var errors = preResult.Select(v => v.Value).GetErrors();

            throw new AggregateException(errors);
        }
    }
}
