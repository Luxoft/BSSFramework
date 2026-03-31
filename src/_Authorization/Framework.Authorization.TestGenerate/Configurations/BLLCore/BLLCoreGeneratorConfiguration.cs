using Framework.Authorization.Domain;
using Framework.CodeGeneration.BLLCoreGenerator.Configuration;

namespace Framework.Authorization.TestGenerate.Configurations.BLLCore;

public class BLLCoreGeneratorConfiguration(AuthorizationGenerationEnvironment environment) : BLLCoreGeneratorConfigurationBase<AuthorizationGenerationEnvironment>(environment)
{
    public override Type ContextFilterModelType { get; } = typeof(DomainObjectContextFilterModel<>);

    public override Type FormatModelType { get; } = typeof(DomainObjectFormatModel<>);

    public override Type ChangeModelType { get; } = typeof(DomainObjectChangeModel<>);
}
