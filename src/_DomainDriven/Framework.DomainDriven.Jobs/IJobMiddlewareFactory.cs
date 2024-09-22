using Framework.DomainDriven.ScopedEvaluate;

namespace Framework.DomainDriven.Jobs;

public interface IJobMiddlewareFactory
{
    IScopedEvaluatorMiddleware Create<TService>(bool withRootLogging);
}
