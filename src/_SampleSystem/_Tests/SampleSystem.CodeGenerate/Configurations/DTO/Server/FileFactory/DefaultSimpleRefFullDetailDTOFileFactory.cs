using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;

namespace SampleSystem.CodeGenerate.ServerDTO;

public class DefaultSimpleRefFullDetailDTOFileFactory<TConfiguration>(TConfiguration configuration, Type domainType) : RefDTOFileFactory<TConfiguration>(configuration, domainType)
    where TConfiguration : class, IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment>
{
    public override MainDTOFileType FileType { get; } = SampleSystemFileType.SimpleRefFullDetailDTO;
}
