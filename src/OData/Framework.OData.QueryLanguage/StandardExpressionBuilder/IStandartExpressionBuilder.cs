namespace Framework.OData.QueryLanguage.StandardExpressionBuilder;

public interface IStandardExpressionBuilder
{
    System.Linq.Expressions.Expression<TDelegate> ToStandardExpression<TDelegate>(LambdaExpression expression);
}
