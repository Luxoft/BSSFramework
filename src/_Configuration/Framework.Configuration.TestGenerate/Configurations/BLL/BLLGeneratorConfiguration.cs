using Framework.CodeGeneration.BLLGenerator.Configuration;
using Framework.Configuration.Domain;

namespace Framework.Configuration.TestGenerate.Configurations.BLL;

public class BLLGeneratorConfiguration(ConfigurationGenerationEnvironment environment) : BLLGeneratorConfigurationBase<ConfigurationGenerationEnvironment>(environment)
{
    public override bool GenerateBllConstructor(Type domainType) => !new[] { typeof(DomainObjectModification), typeof(SystemConstant) }.Contains(domainType) && base.GenerateBllConstructor(domainType);
}
