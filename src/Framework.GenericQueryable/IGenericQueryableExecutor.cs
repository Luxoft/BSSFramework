using System.Linq.Expressions;

namespace Framework.GenericQueryable;

public interface IGenericQueryableExecutor
{
    object Execute(LambdaExpression callExpression);
}
