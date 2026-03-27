namespace Framework.Application.Middleware;

public interface IScopedEvaluatorMiddleware
{
    Task<TResult> EvaluateAsync<TResult>(Func<Task<TResult>> getResult);
}
