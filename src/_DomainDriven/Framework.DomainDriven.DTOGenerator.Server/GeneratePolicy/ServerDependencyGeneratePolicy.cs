using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.Server
{
    public class ServerDependencyGeneratePolicy : DependencyGeneratePolicy
    {
        public ServerDependencyGeneratePolicy(IGeneratePolicy<RoleFileType> baseGeneratePolicy, IEnumerable<GenerateTypeMap> maps)
            : base(baseGeneratePolicy, maps)
        {
        }

        protected override bool InternalUsed(Type domainType, RoleFileType fileType)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));
            if (fileType == null) throw new ArgumentNullException(nameof(fileType));

            if (fileType == ServerFileType.BaseEventDTO)
            {
                return this.Maps.Any(map => map.FileType is DomainOperationEventDTOFileType && this.Used(map.DomainType, map.FileType));
            }
            else if (fileType == ServerFileType.RichEventDTO)
            {
                return this.Maps.Any(map => map.DomainType == domainType && map.FileType is DomainOperationEventDTOFileType && this.Used(map.DomainType, map.FileType))

                    || this.IsUsedProperty(ServerFileType.RichEventDTO, domainType, fileType, true);
            }
            else if (fileType == ServerFileType.SimpleEventDTO)
            {
                return this.IsUsedProperty(ServerFileType.RichEventDTO, domainType, fileType, false);
            }
            else if (base.InternalUsed(domainType, fileType))
            {
                return true;
            }
            else if (fileType == ServerFileType.RichIntegrationDTO)
            {
                return base.InternalUsed(domainType, fileType)

                   || this.IsUsedProperty(ServerFileType.RichIntegrationDTO, domainType, fileType, true);
            }
            else if (fileType == ServerFileType.SimpleIntegrationDTO)
            {
                return this.IsUsedProperty(ServerFileType.RichIntegrationDTO, domainType, fileType, false);
            }
            else
            {
                return false;
            }
        }
    }
}