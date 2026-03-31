using Framework.CodeGeneration.DTOGenerator.Audit.Configuration;

using TypeExtensions = Framework.Projection.TypeExtensions;

namespace SampleSystem.CodeGenerate.Configurations.Services.Audit;

public class AuditDTOGeneratorConfiguration(ServerGenerationEnvironment environment) : AuditDTOGeneratorConfigurationBase<ServerGenerationEnvironment>(environment)
{
    protected override string DomainObjectPropertyRevisionsDTOPrefixName => "SampleSystem";

    protected override string PropertyRevisionDTOPrefixName => "SampleSystem";

    protected override IEnumerable<Type> GetDomainTypes()
    {
        return base.GetDomainTypes().Where(z => !TypeExtensions.IsProjection(z));
    }
}
