using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.Client
{
    public class ClientDependencyGeneratePolicy : DependencyGeneratePolicy
    {
        public ClientDependencyGeneratePolicy(IGeneratePolicy<RoleFileType> baseGeneratePolicy, IEnumerable<GenerateTypeMap> maps)
            : base(baseGeneratePolicy, maps)
        {
        }


        protected override bool InternalUsed(Type domainType, RoleFileType fileType)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));
            if (fileType == null) throw new ArgumentNullException(nameof(fileType));

            if (fileType is MainDTOInterfaceFileType interfaceFileType)
            {
                return this.Used(domainType, interfaceFileType.MainType);
            }
            else if (fileType == ClientFileType.Enum || fileType == ClientFileType.Class || fileType == ClientFileType.Struct)
            {
                return this.IsUsedProperty(null, domainType, fileType);
            }
            else
            {
                return base.InternalUsed(domainType, fileType);
            }
        }
    }
}
