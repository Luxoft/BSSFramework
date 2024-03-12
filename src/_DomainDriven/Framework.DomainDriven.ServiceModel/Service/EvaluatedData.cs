namespace Framework.DomainDriven.ServiceModel.Service;

public class EvaluatedData<TBLLContext, TMappingService>
{
    public EvaluatedData(TBLLContext context, TMappingService mappingService)
    {
        this.Context = context ?? throw new ArgumentNullException(nameof(context));
        this.MappingService = mappingService ?? throw new ArgumentNullException(nameof(mappingService));
    }

    public TBLLContext Context
    {
        get;
    }

    public TMappingService MappingService
    {
        get;
    }
}
