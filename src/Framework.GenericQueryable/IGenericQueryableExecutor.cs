namespace Framework.GenericQueryable;

public interface IGenericQueryableExecutor
{
    object Execute(GenericQueryableMethodExpression genericQueryableMethodExpression);
}
