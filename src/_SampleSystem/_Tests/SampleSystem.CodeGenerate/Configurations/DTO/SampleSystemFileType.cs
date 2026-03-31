using Framework.CodeGeneration.DTOGenerator.FileTypes;

namespace SampleSystem.CodeGenerate;

public static class SampleSystemFileType
{
    public static readonly MainDTOFileType FullRefDTO = new MainDTOFileType(nameof(FullRefDTO), BaseFileType.SimpleDTO, false);

    public static readonly MainDTOFileType SimpleRefFullDetailDTO = new MainDTOFileType(nameof(SimpleRefFullDetailDTO), BaseFileType.FullDTO, false);
}
