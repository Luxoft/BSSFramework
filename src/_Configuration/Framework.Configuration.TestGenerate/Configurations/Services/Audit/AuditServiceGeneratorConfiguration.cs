using Framework.DomainDriven.ServiceModelGenerator;

namespace Framework.Configuration.TestGenerate;

public class AuditServiceGeneratorConfiguration : AuditGeneratorConfigurationBase<ServerGenerationEnvironment>
{
    public AuditServiceGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
    {
    }

    public override string ServiceContractNamespace { get; } = "http://invoicing.luxoft.com/AuditConfigurationFacade";
}
