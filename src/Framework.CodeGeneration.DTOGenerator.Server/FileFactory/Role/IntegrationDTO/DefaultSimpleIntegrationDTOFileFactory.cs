using Framework.CodeGeneration.DTOGenerator.Extensions;
using Framework.CodeGeneration.DTOGenerator.FileType;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Role.IntegrationDTO.Base;
using Framework.CodeGeneration.DTOGenerator.Server.FileType;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Role.IntegrationDTO;

public class DefaultSimpleIntegrationDTOFileFactory<TConfiguration>(TConfiguration configuration, Type domainType)
    : IntegrationDTOFileFactory<TConfiguration>(configuration, domainType)
    where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public override DTOFileType FileType { get; } = ServerFileType.SimpleIntegrationDTO;

    protected override bool HasToDomainObjectMethod => this.IsPersistent();

    protected override bool AllowCreate { get; } = false;
}
