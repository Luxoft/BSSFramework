namespace Framework.DomainDriven.ServiceModel.Service;

public record EvaluatedData<TBllContext, TMappingService>(TBllContext Context, TMappingService MappingService);
