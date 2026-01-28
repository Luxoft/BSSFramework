using Framework.DomainDriven.ServiceModelGenerator;
using Framework.Projection;

namespace SampleSystem.CodeGenerate.Configurations.Services.Audit;

public class AuditServiceGeneratorConfiguration(ServerGenerationEnvironment environment) : AuditGeneratorConfigurationBase<ServerGenerationEnvironment>(environment)
{
    protected override IEnumerable<Type> GetDomainTypes()
    {
        return base.GetDomainTypes().Where(z => !z.IsProjection());
    }
}
