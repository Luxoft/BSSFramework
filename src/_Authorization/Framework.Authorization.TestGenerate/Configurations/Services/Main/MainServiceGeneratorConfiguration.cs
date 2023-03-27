using Framework.DomainDriven.ServiceModelGenerator;

namespace Framework.Authorization.TestGenerate;

public class MainServiceGeneratorConfiguration : MainGeneratorConfigurationBase<ServerGenerationEnvironment>
{
    public MainServiceGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
    {
    }

    public override string ImplementClassName { get; } = "AuthFacade";
}
