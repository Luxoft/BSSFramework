namespace Framework.BLL.ServiceModel.Service;

public record EvaluatedData<TBllContext, TMappingService>(TBllContext Context, TMappingService MappingService);
