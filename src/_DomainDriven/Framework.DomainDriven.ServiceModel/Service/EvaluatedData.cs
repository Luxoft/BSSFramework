namespace Framework.DomainDriven.ServiceModel.Service;

public class EvaluatedData<TBllContext, TMappingService>(TBllContext context, TMappingService mappingService)
{
    public TBllContext Context { get; } = context;

    public TMappingService MappingService { get; } = mappingService;
}
