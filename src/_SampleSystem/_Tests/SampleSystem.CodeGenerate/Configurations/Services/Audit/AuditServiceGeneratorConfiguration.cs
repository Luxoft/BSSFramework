using Framework.DomainDriven.ServiceModelGenerator;

using TypeExtensions = Framework.Projection.TypeExtensions;

namespace SampleSystem.CodeGenerate.Configurations.Services.Audit;

public class AuditServiceGeneratorConfiguration : AuditGeneratorConfigurationBase<ServerGenerationEnvironment>
{
    public AuditServiceGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
    {
    }

    public override string ServiceContractNamespace => "http://sampleSystem.luxoft.com/AuditFacade";

    protected override IEnumerable<Type> GetDomainTypes()
    {
        return base.GetDomainTypes().Where(z => !TypeExtensions.IsProjection(z));
    }
}
