using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;

using SampleSystem.CodeGenerate.Configurations.DTO.Server.FileFactory.Base;

namespace SampleSystem.CodeGenerate.Configurations.DTO.Server.FileFactory;

public class DefaultFullRefDTOFileFactory<TConfiguration>(TConfiguration configuration, Type domainType) : RefDTOFileFactory<TConfiguration>(configuration, domainType)
    where TConfiguration : class, IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment>
{
    public override MainDTOFileType FileType { get; } = SampleSystemFileType.FullRefDTO;
}
