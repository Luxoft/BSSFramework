using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.GeneratePolicy;

using SampleSystem.Domain;

namespace SampleSystem.CodeGenerate;

public class SampleSystemExtGeneratePolicy : IGeneratePolicy<RoleFileType>
{
    public bool Used(Type domainType, RoleFileType fileType)
    {
        if ((fileType == FileType.FullDTO || fileType == FileType.RichDTO) && domainType == typeof(Employee))
        {
            return true;
        }

        if ((fileType == FileType.StrictDTO || fileType == FileType.SimpleDTO)
            && new[] { typeof(TestDefaultFieldsMappingObj), typeof(NoSecurityObject) }.Contains(domainType))
        {
            return true;
        }

        //if (fileType == SampleSystemFileType.SimpleRefFullDetailDTO)
        //{
        //    return true;
        //}

        //if (fileType == SampleSystemFileType.FullRefDTO)
        //{
        //    return true;
        //}

        return false;
    }
}
