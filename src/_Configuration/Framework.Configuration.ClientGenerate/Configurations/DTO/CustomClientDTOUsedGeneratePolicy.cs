using System;
using System.Linq;

using Framework.Configuration.Domain;
using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.Configuration.ClientGenerate
{
    public class CustomClientDTOUsedGeneratePolicy : IGeneratePolicy<RoleFileType>
    {
        public bool Used(Type domainType, RoleFileType identity)
        {
            if (identity == FileType.IdentityDTO)
            {
                return new[] { typeof(Domain.Reports.Report) }.Contains(domainType);
            }

            if (identity == FileType.StrictDTO)
            {
                return new[] { typeof(Attachment) }.Contains(domainType);
            }

            return false;
        }
    }
}
