namespace Framework.Application.ScopedEvaluate;

public interface IScopedEvaluatorMiddleware
{
    Task<TResult> EvaluateAsync<TResult>(Func<Task<TResult>> getResult);
}
