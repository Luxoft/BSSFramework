using Framework.DomainDriven.ScopedEvaluate;

namespace Framework.DomainDriven.Jobs;

public interface IJobMiddlewareFactory
{
    IScopedEvaluatorMiddleware Create<TJob>(bool withRootLogging);
}
