using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators._Base;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Audit;

namespace Framework.CodeGeneration.ServiceModelGenerator.Configuration.Audit;

public abstract class AuditGeneratorConfigurationBase<TEnvironment>(TEnvironment environment)
    : GeneratorConfigurationBase<TEnvironment>(environment), IAuditGeneratorConfigurationBase<TEnvironment>
    where TEnvironment : class, IAuditGenerationEnvironmentBase
{
    public override string ImplementClassName { get; } = "AuditFacade";

    protected override string NamespacePostfix { get; } = "ServiceFacade.Audit";


    public override IEnumerable<IServiceMethodGenerator> GetMethodGenerators(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        foreach (var dtoType in domainType.GetViewDTOTypes())
        {
            yield return new GetObjectWithRevisionMethodGenerator<AuditGeneratorConfigurationBase<TEnvironment>>(this, domainType, dtoType);
        }

        yield return new GetObjectRevisionsMethodGenerator<AuditGeneratorConfigurationBase<TEnvironment>>(this, domainType);

        yield return new GetObjectPropertyRevisionsMethodGenerator<AuditGeneratorConfigurationBase<TEnvironment>>(this, domainType, this.Environment.AuditDTO);

        yield return new GetObjectPropertyRevisionsByDateRangeMethodGenerator<AuditGeneratorConfigurationBase<TEnvironment>>(this, domainType, this.Environment.AuditDTO);
    }
}
