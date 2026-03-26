using Framework.CodeGeneration.DTOGenerator.FileType;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Role.EventDTO.Base;
using Framework.CodeGeneration.DTOGenerator.Server.FileType;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Role.EventDTO;

public class DefaultSimpleEventDTOFileFactory<TConfiguration>(TConfiguration configuration, Type domainType) : EventDTOFileFactory<TConfiguration>(configuration, domainType)
    where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public override DTOFileType FileType { get; } = ServerFileType.SimpleEventDTO;
}
