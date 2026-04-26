using Anch.Core;

namespace Framework.Application.Middleware;

public static class ScopedEvaluatorMiddlewareExtensions
{
    extension(IScopedEvaluatorMiddleware middleware)
    {
        public async Task EvaluateAsync(Func<Task> action) => await middleware.EvaluateAsync(action.ToDefaultTask());

        public IScopedEvaluatorMiddleware With(IScopedEvaluatorMiddleware otherMiddleware) => new WithMiddleware(middleware, otherMiddleware);
    }

    public static IScopedEvaluatorMiddleware Aggregate(this IEnumerable<IScopedEvaluatorMiddleware> middlewareList)
    {
        using var enumerator = middlewareList.GetEnumerator();

        if (enumerator.MoveNext())
        {
            var result = enumerator.Current;

            while (enumerator.MoveNext())
            {
                result = result.With(enumerator.Current);
            }

            return result;
        }
        else
        {
            return new EmptyMiddleware();
        }
    }

    private class EmptyMiddleware : IScopedEvaluatorMiddleware
    {
        public async Task<TResult> EvaluateAsync<TResult>(Func<Task<TResult>> getResult) => await getResult();
    }

    private class WithMiddleware(IScopedEvaluatorMiddleware middleware, IScopedEvaluatorMiddleware otherMiddleware) : IScopedEvaluatorMiddleware
    {
        public async Task<TResult> EvaluateAsync<TResult>(Func<Task<TResult>> getResult) => await otherMiddleware.EvaluateAsync(async () => await middleware.EvaluateAsync(async () => await getResult()));
    }
}
