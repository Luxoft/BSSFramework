namespace Framework.DomainDriven.ScopedEvaluate;

public static class ScopedEvaluatorMiddlewareExtensions
{
    public static IScopedEvaluatorMiddleware With(this IScopedEvaluatorMiddleware middleware, IScopedEvaluatorMiddleware otherMiddleware)
    {
        return new WithMiddleware(middleware, otherMiddleware);
    }

    public static IScopedEvaluatorMiddleware Aggregate(this IEnumerable<IScopedEvaluatorMiddleware> middlewareList)
    {
        return middlewareList.Aggregate((IScopedEvaluatorMiddleware)new EmptyMiddleware(), (m1, m2) => m1.With(m2));
    }


    private class EmptyMiddleware : IScopedEvaluatorMiddleware
    {
        public async Task<TResult> EvaluateAsync<TResult>(Func<Task<TResult>> getResult) => await getResult();
    }

    private class WithMiddleware : IScopedEvaluatorMiddleware
    {
        private readonly IScopedEvaluatorMiddleware middleware;

        private readonly IScopedEvaluatorMiddleware otherMiddleware;

        public WithMiddleware(IScopedEvaluatorMiddleware middleware, IScopedEvaluatorMiddleware otherMiddleware)
        {
            this.middleware = middleware;
            this.otherMiddleware = otherMiddleware;
        }

        public async Task<TResult> EvaluateAsync<TResult>(Func<Task<TResult>> getResult)
        {
            return await this.otherMiddleware.EvaluateAsync(async () => await this.middleware.EvaluateAsync(async () => await getResult()));
        }
    }
}
