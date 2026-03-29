using Framework.CodeGeneration.BLLCoreGenerator.Configuration;
using Framework.Configuration.Domain;

namespace Framework.Configuration.TestGenerate.Configurations.BLLCore;

public partial class BLLCoreGeneratorConfiguration(ServerGenerationEnvironment environment)
    : GeneratorConfigurationBase<ServerGenerationEnvironment>(environment)
{
    public override Type FilterModelType { get; } = typeof(DomainObjectFilterModel<>);

    public override Type CreateModelType { get; } = typeof(DomainObjectCreateModel<>);

    public override Type FormatModelType { get; } = typeof(DomainObjectFormatModel<>);

    public override Type ChangeModelType { get; } = typeof(DomainObjectChangeModel<>);

    public override Type ContextFilterModelType { get; } = typeof(DomainObjectContextFilterModel<>);
}
