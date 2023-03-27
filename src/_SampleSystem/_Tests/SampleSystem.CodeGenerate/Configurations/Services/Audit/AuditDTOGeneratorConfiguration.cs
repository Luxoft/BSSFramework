using Framework.DomainDriven.DTOGenerator.Audit;

using TypeExtensions = Framework.Projection.TypeExtensions;

namespace SampleSystem.CodeGenerate.Configurations.Services.Audit;

public class AuditDTOGeneratorConfiguration : AuditDTOGeneratorConfigurationBase<ServerGenerationEnvironment>
{
    public AuditDTOGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
    {
    }

    protected override string DomainObjectPropertyRevisionsDTOPrefixName => "SampleSystem";

    protected override string PropertyRevisionDTOPrefixName => "SampleSystem";

    protected override IEnumerable<Type> GetDomainTypes()
    {
        return base.GetDomainTypes().Where(z => !TypeExtensions.IsProjection(z));
    }
}
