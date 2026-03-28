using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Role.EventDTO.Base;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Role.EventDTO;

public class DefaultSimpleEventDTOFileFactory<TConfiguration>(TConfiguration configuration, Type domainType) : EventDTOFileFactory<TConfiguration>(configuration, domainType)
    where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public override DTOFileType FileType { get; } = ServerFileType.SimpleEventDTO;
}
