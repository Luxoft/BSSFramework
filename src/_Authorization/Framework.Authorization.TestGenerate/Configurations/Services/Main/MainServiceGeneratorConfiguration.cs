using Framework.CodeGeneration.ServiceModelGenerator.Configuration.Main;

namespace Framework.Authorization.TestGenerate.Configurations.Services.Main;

public class MainServiceGeneratorConfiguration(AuthorizationGenerationEnvironment environment) :
    MainGeneratorConfigurationBase<AuthorizationGenerationEnvironment>(environment)
{
    public override string ImplementClassName { get; } = "AuthFacade";
}
