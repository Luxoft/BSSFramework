using System.Linq.Expressions;

using Framework.Core;

namespace Framework.GenericQueryable.Default;

public class DefaultGenericQueryProvider<T>(IQueryable<T> baseSource) : IQueryProvider
{
    private static readonly IGenericQueryableExecutor GenericQueryableExecutor = new DefaultGenericQueryableExecutor();

    public IQueryable CreateQuery(Expression expression) => throw new NotImplementedException();

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression) =>
        new DefaultGenericQueryable<TElement>(baseSource.Provider.CreateQuery<TElement>(expression));

    public TResult Execute<TResult>(Expression expression) => (TResult)this.Execute(expression);

    public object Execute(Expression expression)
    {
        var visitedExpression = this.CreateVisitor().Visit(expression);

        if (visitedExpression is GenericQueryableExecuteExpression genericQueryableExecuteExpression)
        {
            var pureResult = GenericQueryableExecutor.Execute(genericQueryableExecuteExpression);

            var taskType = genericQueryableExecuteExpression.CallExpression.ReturnType;

            var taskArgType = taskType.GetGenericTypeImplementationArgument(typeof(Task<>));

            return new Func<object, Task<object>>(Task.FromResult)
                   .CreateGenericMethod(taskArgType)
                   .Invoke(null, [pureResult])!;
        }
        else
        {
            return baseSource.Provider.Execute(this.CreateVisitor().Visit(expression))!;
        }
    }

    protected virtual ExpressionVisitor CreateVisitor() => new DefaultGenericQueryableVisitor();
}
