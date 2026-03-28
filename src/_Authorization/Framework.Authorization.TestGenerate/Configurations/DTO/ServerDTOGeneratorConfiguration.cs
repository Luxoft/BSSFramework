using Framework.BLL.Domain.Serialization;

using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.GeneratePolicy;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.GeneratePolicy;

namespace Framework.Authorization.TestGenerate;

public class ServerDTOGeneratorConfiguration(ServerGenerationEnvironment environment) : ServerGeneratorConfigurationBase<ServerGenerationEnvironment>(environment)
{
    public override string DataContractNamespace { get; } = "Auth";

    public override string EventDataContractNamespace { get; } = "http://authorization.luxoft.com/IntegrationEvent";

    public override bool IdentityIsReference { get; } = true;

    protected override IGeneratePolicy<RoleFileType> CreateGeneratePolicy()
    {
        return new DTORoleGeneratePolicy(DTORole.Client | DTORole.Event).Or(new DTORoleGeneratePolicy(DTORole.Event));
    }
}
