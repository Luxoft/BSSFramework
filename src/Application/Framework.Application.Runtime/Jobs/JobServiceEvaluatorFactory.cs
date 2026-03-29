namespace Framework.Application.Jobs;

public class JobServiceEvaluatorFactory(IServiceProvider rootServiceProvider) : IJobServiceEvaluatorFactory
{
    public IJobServiceEvaluator<TService> Create<TService>(bool withRootLogging)
        where TService : notnull =>
        new JobServiceEvaluator<TService>(rootServiceProvider, new JobServiceEvaluatorSettings(withRootLogging));
}
