using System;
using System.Collections.Generic;

using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Client;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.FacadeServiceProxyGenerator
{
    public class ServiceProxyClientDependencyGeneratePolicy : ClientDependencyGeneratePolicy
    {
        public ServiceProxyClientDependencyGeneratePolicy(IGeneratePolicy<RoleFileType> baseGeneratePolicy, IEnumerable<GenerateTypeMap> maps)
                : base(baseGeneratePolicy, maps)
        {
        }

        protected override bool InternalUsed(Type domainType, RoleFileType fileType)
        {
            var baseResult = base.InternalUsed(domainType, fileType);

            if (fileType == DTOGenerator.FileType.RichDTO)
            {
                return baseResult || this.Used(domainType, DTOGenerator.FileType.StrictDTO); // Contravariant methods
            }
            else
            {
                return baseResult;
            }
        }
    }
}
