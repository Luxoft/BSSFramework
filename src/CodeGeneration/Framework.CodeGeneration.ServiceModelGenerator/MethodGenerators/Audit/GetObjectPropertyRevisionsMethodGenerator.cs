using Framework.CodeGeneration.DTOGenerator.Audit.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration.Audit;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Audit._Base;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Audit;

public class GetObjectPropertyRevisionsMethodGenerator<TConfiguration>(TConfiguration configuration, Type domainType, IAuditDTOGeneratorConfiguration dtoConfiguration)
    : GetObjectPropertyRevisionsMethodGeneratorBase<TConfiguration>(configuration, domainType, dtoConfiguration)
    where TConfiguration : class, IAuditGeneratorConfiguration<IAuditGenerationEnvironment>
{
    public override MethodIdentity Identity { get; } = MethodIdentityType.GetPropertyRevisions;

    protected override string Name => $"Get{this.DomainType.Name}PropertyRevisions";


    protected override string GetComment() => $"Get {this.DomainType.Name} Property Revisions";
}
