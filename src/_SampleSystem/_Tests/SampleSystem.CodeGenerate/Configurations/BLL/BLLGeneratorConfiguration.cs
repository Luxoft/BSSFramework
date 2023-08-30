using SampleSystem.Domain;

namespace SampleSystem.CodeGenerate;

public class BLLGeneratorConfiguration : Framework.DomainDriven.BLLGenerator.GeneratorConfigurationBase<
        ServerGenerationEnvironment>
{
    public BLLGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
    {
    }

    /// <summary>
    /// Do not generate BLL Constructors
    /// </summary>
    public override bool GenerateBllConstructor(Type domainType) =>
            domainType switch
            {
                    { } type when type == typeof(Country) => false,
                    _ => base.GenerateBllConstructor(domainType)
            };
}
