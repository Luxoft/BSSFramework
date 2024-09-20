namespace Framework.DomainDriven.Jobs;

public class JobEvaluatorFactory(IServiceProvider rootServiceProvider) : IJobEvaluatorFactory
{
    public IJobEvaluator Create(bool withRootLogging)
    {
        return new JobEvaluator(rootServiceProvider, new JobEvaluatorSettings(withRootLogging));
    }
}
