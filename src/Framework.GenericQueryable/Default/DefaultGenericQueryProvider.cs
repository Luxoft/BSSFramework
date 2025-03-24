using System.Linq.Expressions;

using Framework.Core;

namespace Framework.GenericQueryable.Default;

public class DefaultGenericQueryProvider<T>(IQueryable<T> baseSource) : IQueryProvider
{
    private readonly IGenericQueryableExecutor genericQueryableExecutor = new DefaultGenericQueryableExecutor();

    public IQueryable CreateQuery(Expression expression) => throw new NotImplementedException();

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression) =>
        new DefaultGenericQueryable<TElement>(baseSource.Provider.CreateQuery<TElement>(expression));

    public TResult Execute<TResult>(Expression expression) => (TResult)this.Execute(expression);

    public object Execute(Expression expression)
    {
        if (expression is GenericQueryableExecuteExpression genericQueryableExecuteExpression)
        {
            var pureResult = this.genericQueryableExecutor.Execute(genericQueryableExecuteExpression.CallExpression);

            var returnType = genericQueryableExecuteExpression.CallExpression.ReturnType;

            if (returnType.IsGenericTypeImplementation(typeof(Task<>)))
            {
                var taskArgType = returnType.GetGenericTypeImplementationArgument(typeof(Task<>));

                return new Func<object, Task<object>>(Task.FromResult)
                       .CreateGenericMethod(taskArgType)
                       .Invoke(null, [pureResult])!;
            }
            else
            {
                return pureResult;
            }
        }
        else
        {
            return baseSource.Provider.Execute(expression)!;
        }
    }
}
