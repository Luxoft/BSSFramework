namespace Framework.GenericQueryable;

public interface IGenericQueryableExecutor
{
    object Execute(GenericQueryableExecuteExpression genericQueryableExecuteExpression);
}
