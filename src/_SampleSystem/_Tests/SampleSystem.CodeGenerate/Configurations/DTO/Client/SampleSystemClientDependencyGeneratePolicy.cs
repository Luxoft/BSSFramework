using System;
using System.Collections.Generic;

using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Client;
using Framework.DomainDriven.Generation.Domain;

namespace SampleSystem.CodeGenerate.ClientDTO
{
    public class SampleSystemClientDependencyGeneratePolicy : ClientDependencyGeneratePolicy
    {
        public SampleSystemClientDependencyGeneratePolicy(IGeneratePolicy<RoleFileType> baseGeneratePolicy, IEnumerable<GenerateTypeMap> maps)
            : base(baseGeneratePolicy, maps)
        {
        }

        protected override bool InternalUsed(Type domainType, RoleFileType fileType)
        {
            if (base.InternalUsed(domainType, fileType))
            {
                return true;
            }
            else if (fileType == FileType.SimpleDTO)
            {
                return base.InternalUsed(domainType, fileType) || this.Used(domainType, SampleSystemFileType.FullRefDTO)
                       || this.IsUsedProperty(SampleSystemFileType.FullRefDTO, domainType, fileType);
            }
            else if (fileType == FileType.FullDTO)
            {
                return base.InternalUsed(domainType, fileType) || this.IsUsedProperty(SampleSystemFileType.FullRefDTO, domainType, fileType);
            }
            else
            {
                return false;
            }
        }
    }
}