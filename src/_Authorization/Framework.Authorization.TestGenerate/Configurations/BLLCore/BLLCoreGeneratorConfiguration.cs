using Framework.Authorization.Domain;
using Framework.DomainDriven.BLLCoreGenerator;

namespace Framework.Authorization.TestGenerate;

public class BLLCoreGeneratorConfiguration : GeneratorConfigurationBase<ServerGenerationEnvironment>
{
    public BLLCoreGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
    {
    }

    public override Type FilterModelType { get; } = typeof(DomainObjectFilterModel<>);

    public override Type ODataFilterModelType { get; } = typeof(DomainObjectODataFilterModel<>);

    public override Type ODataContextFilterModelType { get; } = typeof(DomainObjectODataContextFilterModel<>);

    public override Type ContextFilterModelType { get; } = typeof(DomainObjectContextFilterModel<>);

    public override Type CreateModelType { get; } = typeof(DomainObjectCreateModel<>);

    public override Type FormatModelType { get; } = typeof(DomainObjectFormatModel<>);

    public override Type ChangeModelType { get; } = typeof(DomainObjectChangeModel<>);

    public override bool GenerateDomainServiceConstructor(Type domainType)
    {
        return !new[] { typeof(Principal), typeof(Permission), typeof(BusinessRole) }.Contains(domainType);
    }
}
