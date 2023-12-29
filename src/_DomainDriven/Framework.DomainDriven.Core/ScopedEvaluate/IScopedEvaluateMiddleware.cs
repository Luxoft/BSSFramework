namespace Framework.DomainDriven.ScopedEvaluate;

public interface IScopedEvaluatorMiddleware
{
    Task<TResult> EvaluateAsync<TResult>(Func<Task<TResult>> getResult);
}
