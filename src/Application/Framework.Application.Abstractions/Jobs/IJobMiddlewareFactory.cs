using Framework.Application.Middleware;

namespace Framework.Application.Jobs;

public interface IJobMiddlewareFactory
{
    IScopedEvaluatorMiddleware Create<TService>(bool withRootLogging);
}
