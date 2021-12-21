using System;
using System.Linq;

using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.Generation.Domain;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.ClientGenerate
{
    public class CustomClientDTOUsedGeneratePolicy : IGeneratePolicy<RoleFileType>
    {
        public bool Used(Type domainType, RoleFileType identity)
        {
            if (identity == FileType.RichDTO)
            {
                return new[] { typeof(StateBase) }.Contains(domainType);
            }

            return false;
        }
    }
}
