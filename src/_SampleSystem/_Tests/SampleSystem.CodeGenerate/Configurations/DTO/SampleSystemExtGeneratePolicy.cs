using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.GeneratePolicy;

using SampleSystem.Domain;

namespace SampleSystem.CodeGenerate;

public class SampleSystemExtGeneratePolicy : IGeneratePolicy<RoleFileType>
{
    public bool Used(Type domainType, RoleFileType fileType)
    {
        if ((fileType == BaseFileType.FullDTO || fileType == BaseFileType.RichDTO) && domainType == typeof(Employee))
        {
            return true;
        }

        if ((fileType == BaseFileType.StrictDTO || fileType == BaseFileType.SimpleDTO)
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
