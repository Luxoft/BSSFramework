using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Server;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Serialization;

namespace Framework.Authorization.TestGenerate;

public class ServerDTOGeneratorConfiguration(ServerGenerationEnvironment environment) : ServerGeneratorConfigurationBase<ServerGenerationEnvironment>(environment)
{
    public override string DataContractNamespace => this.Environment.DTODataContractNamespace;

    public override string EventDataContractNamespace { get; } = "http://authorization.luxoft.com/IntegrationEvent";

    public override bool IdentityIsReference { get; } = true;

    protected override IGeneratePolicy<RoleFileType> CreateGeneratePolicy()
    {
        return new DTORoleGeneratePolicy(DTORole.Client | DTORole.Event).Or(new DTORoleGeneratePolicy(DTORole.Event));
    }
}
