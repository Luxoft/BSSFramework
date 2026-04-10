using Framework.CodeGeneration.ServiceModelGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators._Base;

namespace Framework.CodeGeneration.ServiceModelGenerator.Configuration.Query;

public abstract class QueryGeneratorConfigurationBase<TEnvironment>(TEnvironment environment) : ServiceModelGeneratorBase<TEnvironment>(environment)
    where TEnvironment : class, IServiceModelGenerationEnvironment
{
    public override string ImplementClassName { get; } = "QueryFacade";

    protected override string NamespacePostfix { get; } = "ServiceFacade.Query";


    public override IEnumerable<IServiceMethodGenerator> GetMethodGenerators(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        foreach (var dtoType in this.Environment.MetadataProxyProvider.Wrap(domainType).GetViewDTOTypes())
        {
            foreach (var methodGenerator in new QueryServiceMethodGeneratorCollection<QueryGeneratorConfigurationBase<TEnvironment>>(this, domainType, dtoType))
            {
                yield return methodGenerator;
            }
        }
    }
}
