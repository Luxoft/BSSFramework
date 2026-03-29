namespace Framework.Infrastructure.Service;

public record EvaluatedData<TBllContext, TMappingService>(TBllContext Context, TMappingService MappingService);
