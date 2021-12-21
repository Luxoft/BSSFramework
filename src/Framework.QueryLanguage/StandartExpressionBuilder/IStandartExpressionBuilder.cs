namespace Framework.QueryLanguage
{
    public interface IStandartExpressionBuilder
    {
        System.Linq.Expressions.Expression<TDelegate> ToStandartExpression<TDelegate>(LambdaExpression expression);
    }
}