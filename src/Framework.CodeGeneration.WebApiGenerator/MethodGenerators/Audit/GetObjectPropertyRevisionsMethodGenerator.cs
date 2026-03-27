using Framework.CodeGeneration.DTOGenerator.Audit.Configuration;
using Framework.CodeGeneration.WebApiGenerator.Configuration.Audit;

using Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Audit._Base;

namespace Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Audit;

public class GetObjectPropertyRevisionsMethodGenerator<TConfiguration> : GetObjectPropertyRevisionsMethodGeneratorBase<TConfiguration>
        where TConfiguration : class, IAuditGeneratorConfigurationBase<IAuditGenerationEnvironmentBase>
{
    public GetObjectPropertyRevisionsMethodGenerator(TConfiguration configuration, Type domainType, IAuditDTOGeneratorConfigurationBase dtoConfiguration)
            : base(configuration, domainType, dtoConfiguration)
    {
    }

    public override MethodIdentity Identity { get; } = MethodIdentityType.GetPropertyRevisions;

    protected override string Name => $"Get{this.DomainType.Name}PropertyRevisions";


    protected override string GetComment()
    {
        return $"Get {this.DomainType.Name} Property Revisions";
    }
}
