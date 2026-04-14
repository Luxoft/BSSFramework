using Framework.CodeGeneration.ServiceModelGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Audit;

namespace Framework.CodeGeneration.ServiceModelGenerator.Configuration.Audit;

public abstract class AuditGeneratorConfigurationBase<TEnvironment>(TEnvironment environment)
    : ServiceModelGeneratorBase<TEnvironment>(environment), IAuditGeneratorConfiguration<TEnvironment>
    where TEnvironment : class, IAuditGenerationEnvironment
{
    public override string ImplementClassName { get; } = "AuditFacade";

    protected override string NamespacePostfix { get; } = "ServiceFacade.Audit";


    public override IEnumerable<IServiceMethodGenerator> GetMethodGenerators(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        foreach (var dtoType in this.Environment.MetadataProxyProvider.Wrap(domainType).GetViewDTOTypes())
        {
            yield return new GetObjectWithRevisionMethodGenerator<AuditGeneratorConfigurationBase<TEnvironment>>(this, domainType, dtoType);
        }

        yield return new GetObjectRevisionsMethodGenerator<AuditGeneratorConfigurationBase<TEnvironment>>(this, domainType);

        yield return new GetObjectPropertyRevisionsMethodGenerator<AuditGeneratorConfigurationBase<TEnvironment>>(this, domainType, this.Environment.AuditDTO);

        yield return new GetObjectPropertyRevisionsByDateRangeMethodGenerator<AuditGeneratorConfigurationBase<TEnvironment>>(this, domainType, this.Environment.AuditDTO);
    }
}
