namespace Framework.Infrastructure.Service;

public record EvaluatedData<TBLLContext, TMappingService>(TBLLContext Context, TMappingService MappingService);
