using System;
using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.Generation.Domain;

using WorkflowSampleSystem.Domain;

namespace WorkflowSampleSystem.CodeGenerate
{
    public class WorkflowSampleSystemExtGeneratePolicy : IGeneratePolicy<RoleFileType>
    {
        public bool Used(Type domainType, RoleFileType fileType)
        {
            if ((fileType == FileType.FullDTO || fileType == FileType.RichDTO) && domainType == typeof(Employee))
            {
                return true;
            }

            if ((fileType == FileType.StrictDTO || fileType == FileType.SimpleDTO) && domainType == typeof(TestDefaultFieldsMappingObj))
            {
                return true;
            }

            //if (fileType == WorkflowSampleSystemFileType.SimpleRefFullDetailDTO)
            //{
            //    return true;
            //}

            //if (fileType == WorkflowSampleSystemFileType.FullRefDTO)
            //{
            //    return true;
            //}

            return false;
        }
    }
}
