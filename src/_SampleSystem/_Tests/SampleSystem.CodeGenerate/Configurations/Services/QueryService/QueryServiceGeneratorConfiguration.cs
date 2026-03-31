using Framework.CodeGeneration.GeneratePolicy;
using Framework.CodeGeneration.ServiceModelGenerator;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration.Query;

namespace SampleSystem.CodeGenerate;

public class QueryServiceGeneratorConfiguration : QueryGeneratorConfigurationBase<ServerGenerationEnvironment>
{
    public QueryServiceGeneratorConfiguration(ServerGenerationEnvironment environment)
        : base(environment)
    {
        this.GeneratePolicy =
            base.GeneratePolicy
                .Add(
                    (domainType, methodIdentity) =>
                        domainType.Name == nameof(SampleSystemProjectionSource.TestBusinessUnit)
                        && methodIdentity.Type == MethodIdentityType.GetODataTreeByQueryStringWithOperation);
    }

    public override IGeneratePolicy<MethodIdentity> GeneratePolicy { get; }
}
