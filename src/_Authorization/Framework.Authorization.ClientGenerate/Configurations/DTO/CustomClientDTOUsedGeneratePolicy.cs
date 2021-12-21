using System;

using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.Authorization.ClientGenerate
{
    public class CustomClientDTOUsedGeneratePolicy : IGeneratePolicy<RoleFileType>
    {
        public bool Used(Type domainType, RoleFileType identity)
        {
            return false;
        }
    }
}
