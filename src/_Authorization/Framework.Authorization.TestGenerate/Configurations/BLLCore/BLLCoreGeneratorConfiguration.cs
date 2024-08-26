using Framework.Authorization.Domain;
using Framework.DomainDriven.BLLCoreGenerator;

namespace Framework.Authorization.TestGenerate;

public class BLLCoreGeneratorConfiguration(ServerGenerationEnvironment environment)
    : GeneratorConfigurationBase<ServerGenerationEnvironment>(environment)
{
    public override Type ContextFilterModelType { get; } = typeof(DomainObjectContextFilterModel<>);

    public override Type FormatModelType { get; } = typeof(DomainObjectFormatModel<>);

    public override Type ChangeModelType { get; } = typeof(DomainObjectChangeModel<>);
}
