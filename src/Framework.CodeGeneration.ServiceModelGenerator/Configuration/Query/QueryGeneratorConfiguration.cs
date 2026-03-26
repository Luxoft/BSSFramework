namespace Framework.DomainDriven.ServiceModelGenerator;

public abstract class QueryGeneratorConfigurationBase<TEnvironment> : GeneratorConfigurationBase<TEnvironment>
        where TEnvironment : class, IGenerationEnvironmentBase
{
    protected QueryGeneratorConfigurationBase(TEnvironment environment)
            : base(environment)
    {
    }


    public override string ImplementClassName { get; } = "QueryFacade";

    protected override string NamespacePostfix { get; } = "ServiceFacade.Query";


    public override IEnumerable<IServiceMethodGenerator> GetMethodGenerators(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        foreach (var dtoType in domainType.GetViewDTOTypes())
        {
            foreach (var methodGenerator in new QueryServiceMethodGeneratorCollection<QueryGeneratorConfigurationBase<TEnvironment>>(this, domainType, dtoType))
            {
                yield return methodGenerator;
            }
        }
    }
}
