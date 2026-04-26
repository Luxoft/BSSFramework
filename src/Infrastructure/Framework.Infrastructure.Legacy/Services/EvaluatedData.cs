namespace Framework.Infrastructure.Services;

public record EvaluatedData<TBLLContext, TMappingService>(TBLLContext Context, TMappingService MappingService);
