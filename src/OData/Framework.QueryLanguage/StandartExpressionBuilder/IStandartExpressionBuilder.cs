namespace Framework.QueryLanguage;

public interface IStandardExpressionBuilder
{
    System.Linq.Expressions.Expression<TDelegate> ToStandardExpression<TDelegate>(LambdaExpression expression);
}
