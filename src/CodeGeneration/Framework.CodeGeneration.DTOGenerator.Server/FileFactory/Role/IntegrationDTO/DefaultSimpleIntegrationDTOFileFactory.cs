using Framework.CodeGeneration.DTOGenerator.Extensions;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Role.IntegrationDTO.Base;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Role.IntegrationDTO;

public class DefaultSimpleIntegrationDTOFileFactory<TConfiguration>(TConfiguration configuration, Type domainType)
    : IntegrationDTOFileFactory<TConfiguration>(configuration, domainType)
    where TConfiguration : class, IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment>
{
    public override DTOFileType FileType { get; } = ServerFileType.SimpleIntegrationDTO;

    protected override bool HasToDomainObjectMethod => this.IsPersistent();

    protected override bool AllowCreate { get; } = false;
}
