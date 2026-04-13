using Framework.CodeGeneration.DTOGenerator.Audit.Configuration;
using Framework.Projection;

namespace SampleSystem.CodeGenerate.Configurations.Services.Audit;

public class AuditDTOGeneratorConfiguration(ServerGenerationEnvironment environment) : AuditDTOGeneratorConfigurationBase<ServerGenerationEnvironment>(environment)
{
    protected override string DomainObjectPropertyRevisionsDTOPrefixName => "SampleSystem";

    protected override string PropertyRevisionDTOPrefixName => "SampleSystem";

    protected override IEnumerable<Type> GetDomainTypes() => base.GetDomainTypes().Where(v => !v.IsProjection());
}
