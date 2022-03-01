using System;

using Framework.DomainDriven.DTOGenerator;

namespace SampleSystem.CodeGenerate
{
    public static class SampleSystemFileType
    {
        public static readonly MainDTOFileType FullRefDTO = new MainDTOFileType(() => FullRefDTO, () => FileType.SimpleDTO, () => null, false);

        public static readonly MainDTOFileType SimpleRefFullDetailDTO = new MainDTOFileType(() => SimpleRefFullDetailDTO, () => FileType.FullDTO, () => null, false);
    }
}