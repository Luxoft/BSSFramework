using Framework.Configuration.Domain;

using Framework.DomainDriven.BLLCoreGenerator;

namespace Framework.Configuration.TestGenerate;

public partial class BLLCoreGeneratorConfiguration(ServerGenerationEnvironment environment)
    : GeneratorConfigurationBase<ServerGenerationEnvironment>(environment)
{
    public override Type FilterModelType { get; } = typeof(DomainObjectFilterModel<>);

    public override Type CreateModelType { get; } = typeof(DomainObjectCreateModel<>);

    public override Type FormatModelType { get; } = typeof(DomainObjectFormatModel<>);

    public override Type ChangeModelType { get; } = typeof(DomainObjectChangeModel<>);

    public override Type ContextFilterModelType { get; } = typeof(DomainObjectContextFilterModel<>);
}
