using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Server;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Serialization;

namespace Framework.Authorization.TestGenerate;

public class ServerDTOGeneratorConfiguration : ServerGeneratorConfigurationBase<ServerGenerationEnvironment>
{
    public ServerDTOGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
    {
    }

    public override string DataContractNamespace => this.Environment.DTODataContractNamespace;

    public override string EventDataContractNamespace { get; } = "http://authorization.luxoft.com/IntegrationEvent";

    public override bool IdentityIsReference { get; } = true;

    protected override IGeneratePolicy<RoleFileType> CreateGeneratePolicy()
    {
        return new DTORoleGeneratePolicy(DTORole.Client | DTORole.Event).Or(new DTORoleGeneratePolicy(DTORole.Event));
    }

    public override bool ForceGenerateProperties(Type domainType, DTOFileType fileType)
    {
        return true;
    }
}
