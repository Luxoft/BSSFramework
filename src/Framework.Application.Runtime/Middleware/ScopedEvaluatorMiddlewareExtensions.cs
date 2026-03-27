using Framework.Core;

namespace Framework.Application.Middleware;

public static class ScopedEvaluatorMiddlewareExtensions
{
    extension(IScopedEvaluatorMiddleware middleware)
    {
        public async Task EvaluateAsync(Func<Task> action) => await middleware.EvaluateAsync(action.ToDefaultTask());

        public IScopedEvaluatorMiddleware With(IScopedEvaluatorMiddleware otherMiddleware) => new WithMiddleware(middleware, otherMiddleware);
    }

    public static IScopedEvaluatorMiddleware Aggregate(this IEnumerable<IScopedEvaluatorMiddleware> middlewareList) => middlewareList.Aggregate((IScopedEvaluatorMiddleware)new EmptyMiddleware(), (m1, m2) => m1.With(m2));

    private class EmptyMiddleware : IScopedEvaluatorMiddleware
    {
        public async Task<TResult> EvaluateAsync<TResult>(Func<Task<TResult>> getResult) => await getResult();
    }

    private class WithMiddleware(IScopedEvaluatorMiddleware middleware, IScopedEvaluatorMiddleware otherMiddleware) : IScopedEvaluatorMiddleware
    {
        public async Task<TResult> EvaluateAsync<TResult>(Func<Task<TResult>> getResult) => await otherMiddleware.EvaluateAsync(async () => await middleware.EvaluateAsync(async () => await getResult()));
    }
}
