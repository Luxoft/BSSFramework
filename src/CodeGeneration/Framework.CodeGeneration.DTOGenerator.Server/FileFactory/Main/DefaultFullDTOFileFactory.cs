using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Main.Base;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Main;

public class DefaultFullDTOFileFactory<TConfiguration>(TConfiguration configuration, Type domainType) : MainDTOFileFactory<TConfiguration>(configuration, domainType)
    where TConfiguration : class, IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment>
{
    public override MainDTOFileType FileType { get; } = BaseFileType.FullDTO;
}
