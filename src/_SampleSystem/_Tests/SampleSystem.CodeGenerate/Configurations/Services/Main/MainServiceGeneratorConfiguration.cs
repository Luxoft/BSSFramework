using Framework.CodeGeneration.GeneratePolicy;
using Framework.CodeGeneration.ServiceModelGenerator;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration.Main;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators;
using Framework.FileGeneration.Configuration;
using Framework.Projection;

namespace SampleSystem.CodeGenerate.Configurations.Services.Main;

/// <summary>
/// Configuration for generating the main facade of the SampleSystem system (an example with added generation of facade methods from a ComplexChange model)
/// </summary>
public class MainServiceGeneratorConfiguration : MainGeneratorConfigurationBase<ServerGenerationEnvironment>
{
    public MainServiceGeneratorConfiguration(ServerGenerationEnvironment environment)
        : base(environment) =>
        this.GeneratePolicy = new CustomServiceGeneratePolicy(this.Environment);

    public override IGeneratePolicy<MethodIdentity> GeneratePolicy { get; }


    public override IEnumerable<IServiceMethodGenerator> GetMethodGenerators(Type domainType)
    {
        foreach (var method in base.GetMethodGenerators(domainType))
        {
            yield return method;
        }

        if (!domainType.IsProjection())
        {
            foreach (var complexChangeModelType in this.Environment.GetModelTypes(domainType, this.Environment.BLLCore.ComplexChangeModelType))
            {
                yield return new ComplexChangeMethodGenerator(this, domainType, complexChangeModelType);
            }
        }
    }
}
