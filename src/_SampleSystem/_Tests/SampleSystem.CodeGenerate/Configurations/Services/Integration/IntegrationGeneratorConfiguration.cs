using Framework.DomainDriven.ServiceModelGenerator;
using Framework.SecuritySystem;

using SampleSystem.Security;

namespace SampleSystem.CodeGenerate;

public class IntegrationGeneratorConfiguration : IntegrationGeneratorConfigurationBase<ServerGenerationEnvironment>
{
    public IntegrationGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
    {
    }

    public override SecurityRole IntegrationSecurityRole { get; } = SampleSystemSecurityRole.SystemIntegration;

    public override Type IntegrationSecurityRoleType { get; } = typeof(SampleSystemSecurityRole);
}
