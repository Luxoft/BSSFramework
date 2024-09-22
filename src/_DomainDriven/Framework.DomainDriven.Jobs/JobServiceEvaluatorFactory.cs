namespace Framework.DomainDriven.Jobs;

public class JobServiceEvaluatorFactory(IServiceProvider rootServiceProvider) : IJobServiceEvaluatorFactory
{
    public IJobServiceEvaluator<TService> Create<TService>(bool withRootLogging)
        where TService : notnull
    {
        return new JobServiceEvaluator<TService>(rootServiceProvider, new JobServiceEvaluatorSettings(withRootLogging));
    }
}
