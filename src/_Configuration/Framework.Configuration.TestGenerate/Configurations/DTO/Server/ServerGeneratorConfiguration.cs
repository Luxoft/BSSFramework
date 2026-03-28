using Framework.BLL.Domain.Serialization;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.GeneratePolicy;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.GeneratePolicy;
using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Server;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Serialization;

namespace Framework.Configuration.TestGenerate;

public class ServerDTOGeneratorConfiguration : ServerGeneratorConfigurationBase<ServerGenerationEnvironment>
{
    public ServerDTOGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
    {
    }

    public override ClientDTORole MapToDomainRole { get; } = ClientDTORole.All;

    public override string DataContractNamespace => this.Environment.DTODataContractNamespace;

    public override string Namespace { get; } = "Framework.Configuration.Generated.DTO";

    public override Type VersionType => typeof(long);

    public override bool IdentityIsReference { get; } = true;

    protected override IGeneratePolicy<RoleFileType> CreateGeneratePolicy()
    {
        return new DTORoleGeneratePolicy(DTORole.Client);
    }
}
