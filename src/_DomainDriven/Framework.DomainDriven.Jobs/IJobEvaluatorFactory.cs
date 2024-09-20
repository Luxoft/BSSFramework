namespace Framework.DomainDriven.Jobs;

public interface IJobEvaluatorFactory
{
    IJobEvaluator Create(bool withRootLogging);
}
