namespace Framework.DomainDriven.ServiceModel.Service;

public class EvaluatedData<TBLLContext>
{
    public EvaluatedData(TBLLContext context)
    {
        this.Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public TBLLContext Context
    {
        get;
    }
}

public class EvaluatedData<TBLLContext, TDTOMappingService> : EvaluatedData<TBLLContext>
{
    public EvaluatedData(TBLLContext context, TDTOMappingService mappingService)
            : base(context)
    {
        this.MappingService = mappingService ?? throw new ArgumentNullException(nameof(mappingService));
    }

    public TDTOMappingService MappingService
    {
        get;
    }
}
